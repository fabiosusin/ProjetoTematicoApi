using DAO.DBConnection;
using DAO.Intra.PersonDAO;
using DAO.Intra.SituationDAO;
using DTO.General.Base.Api.Output;
using DTO.Intra.Situation.Database;
using DTO.Intra.Situation.Output;
using System;
using System.Linq;

namespace Business.API.Intra.BlSituation
{
    public class BlSituation
    {
        private readonly SituationDAO SituationDAO;
        private readonly PersonDAO IntraPersonDAO;
        public BlSituation(XDataDatabaseSettings settings)
        {
            SituationDAO = new(settings);
            IntraPersonDAO = new(settings);
        }

        public BaseApiOutput UpsertSituation(Situation input)
        {
            input.PersonId = IntraPersonDAO.FindOne(x => x.CpfCnpj == input.PersonDocument)?.Id ?? 0;
            var baseValidation = BasicValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            var result = input.Id == 0 ? SituationDAO.Insert(input) : SituationDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar uma nova Situação Processual!") : new(true);
        }

        public Situation GetSituation(int processNumber) => processNumber == 0 ? null : SituationDAO.FindOne(x => x.ProcessNumber == processNumber);

        public BaseApiOutput DeleteSituation(int id)
        {
            if (id == 0)
                return new("Requisição mal formada!");

            var Situation = SituationDAO.FindById(id);
            if (Situation == null)
                return new("Situação não encontrada!");

            SituationDAO.Remove(Situation);
            return new(true);
        }

        public SituationListOutput List()
        {
            var result = SituationDAO.List();
            if (!(result?.Any() ?? false))
                return new("Nenhuma Situação encontrada!");

            return new(result);
        }

        private BaseApiOutput BasicValidation(Situation input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.PersonDocument))
                return new("Documento da Pessoa não informado!");

            if (IntraPersonDAO.FindOne(x => x.CpfCnpj == input.PersonDocument) == null)
                return new("Pessoa não cadastrada no sistema!");

            if (SituationDAO.FindOne(x => x.ProcessNumber == input.ProcessNumber && x.Id != input.Id) != null)
                return new("Já existe uma Situação Processual cadastrada com este Número de Processo");

            if (SituationDAO.FindOne(x => x.PersonId == input.PersonId && x.Id != input.Id) != null)
                return new("Pessoa já está vinculada a uma Situação Processual!");

            if (input.RemainingHours <= 0)
                return new("Horas a cumprir não informadas!");

            return new(true);
        }
    }
}
