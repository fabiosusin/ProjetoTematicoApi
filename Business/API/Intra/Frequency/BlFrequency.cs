using DAO.DBConnection;
using DAO.Intra.FrequencyDAO;
using DAO.Intra.PersonDAO;
using DAO.Intra.SituationDAO;
using DTO.General.Base.Api.Output;
using DTO.Intra.Frequency.Output;
using DTO.Intra.FrequencyDB.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.API.Intra.BlFrequency
{
    public class BlFrequency
    {
        private readonly FrequencyDAO FrequencyDAO;
        private readonly SituationDAO SituationDAO;
        private readonly PersonDAO IntraPersonDAO;
        public BlFrequency(XDataDatabaseSettings settings)
        {
            FrequencyDAO = new(settings);
            SituationDAO = new(settings);
            IntraPersonDAO = new(settings);
        }

        public BaseApiOutput UpsertFrequency(Frequency input)
        {
            input.PersonId = IntraPersonDAO.FindOne(x => x.CpfCnpj == input.PersonDocument)?.Id ?? Guid.Empty;
            var baseValidation = BasicValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            var sit = SituationDAO.FindOne(x => x.PersonId == input.PersonId);
            if (sit == null)
                return new("Situação processual não encontrada para esta Pessoa!");
            
            var totalTime = (int)(input.ExitTime - input.EntryTime).TotalHours;
            sit.FulfilledHours += totalTime;
            sit.RemainingHours -= totalTime;
            if (sit.RemainingHours < 0)
                sit.RemainingHours = 0;

            SituationDAO.Update(sit);
            input.RemainingHours = sit.RemainingHours;
            input.FulfilledHours = sit.FulfilledHours;
            input.ActivityTotalTime = totalTime;

            var result = input.Id == Guid.Empty ? FrequencyDAO.Insert(input) : FrequencyDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar uma nova Frequência!") : new(true);
        }

        public Frequency GetFrequency(Guid id) => id == Guid.Empty ? null : FrequencyDAO.FindOne(x => x.Id == id);

        public BaseApiOutput DeleteFrequency(Guid id)
        {
            if (id == Guid.Empty)
                return new("Requisição mal formada!");

            var frequency = FrequencyDAO.FindById(id);
            if (frequency == null)
                return new("Parceiro não encontrado!");

            var sit = SituationDAO.FindOne(x => x.PersonId == frequency.PersonId);
            if (sit == null)
                return new("Situação processual não encontrada para esta Pessoa!");

            sit.FulfilledHours -= frequency.ActivityTotalTime;
            sit.RemainingHours += frequency.ActivityTotalTime;
            SituationDAO.Update(sit);
            FrequencyDAO.Remove(frequency);
            return new(true);
        }

        public FrequencyListOutput List()
        {
            var result = FrequencyDAO.List();
            if (!(result?.Any() ?? false))
                return new("Nenhuma Frequência encontrada!");

            return new(result);
        }

        public IEnumerable<Frequency> Export() => FrequencyDAO.List();

        private BaseApiOutput BasicValidation(Frequency input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (input.Id != Guid.Empty)
                return new("Não é possível editar uma frequência!");

            if (string.IsNullOrEmpty(input.PersonDocument))
                return new("Documento da Pessoa não informado!");

            if (IntraPersonDAO.FindOne(x => x.CpfCnpj == input.PersonDocument) == null)
                return new("Pessoa não cadastrada no sistema!");

            if (string.IsNullOrEmpty(input.Activity))
                return new("Atividade não informada!");

            if (!input.Appear)
                return new(true);

            if (input.EntryTime == DateTime.MinValue)
                return new("Data de Entrada não informada corretamente!");

            if (input.ExitTime == DateTime.MinValue)
                return new("Data de Saída não informada corretamente!");

            if (input.EntryTime > input.ExitTime)
                return new("Data de Entrada maior que a data de Saída!");

            return new(true);
        }
    }
}
