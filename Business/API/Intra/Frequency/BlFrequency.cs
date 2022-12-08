using Business.General;
using DAO.DBConnection;
using DAO.Intra.FrequencyDAO;
using DAO.Intra.PersonDAO;
using DAO.Intra.SituationDAO;
using DTO.General.Api.Input;
using DTO.General.Api.Output;
using DTO.General.Base.Api.Output;
using DTO.General.Excel.Input;
using DTO.Intra.Frequency.Output;
using DTO.Intra.FrequencyDB.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

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
            input.PersonId = IntraPersonDAO.FindOne(x => x.CpfCnpj == input.PersonDocument)?.Id ?? 0;
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

            var result = input.Id == 0 ? FrequencyDAO.Insert(input) : FrequencyDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar uma nova Frequência!") : new(true);
        }

        public Frequency GetFrequency(int id) => id == 0 ? null : FrequencyDAO.FindOne(x => x.Id == id);

        public BaseApiOutput DeleteFrequency(int id)
        {
            if (id == 0)
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

        public GenerateDocOutput Export()
        {
            var data = FrequencyDAO.List();
            if (!(data?.Any() ?? false))
                return null;

            var result = new List<Frequency>();
            foreach (var item in data)
                result.Add(CryptographyService.EncryptFrequency(item));

            var path = "C:\\frequency.json";
            File.WriteAllText(path, JsonConvert.SerializeObject(result));

            return new("Exportação_Frequência", path);
        }

        public BaseApiOutput Import(ImportFileInput input)
        {
            if (string.IsNullOrEmpty(input?.DataBase64))
                return new("Dados inválidos!");

            try
            {
                var byteArray = Convert.FromBase64String(new Regex("data:application/json;base64,").Replace(input.DataBase64, ""));
                var frequencies = JsonConvert.DeserializeObject<List<Frequency>>(Encoding.UTF8.GetString(byteArray));
                foreach(var frequency in frequencies)
                    FrequencyDAO.Upsert(CryptographyService.DecryptFrequency(frequency));

            }
            catch { return new("Dados em formato inválido!"); }

            return new(true);
        }

        public GenerateDocOutput GetExcel()
        {
            var data = List();
            if (!(data?.Success ?? false))
                return null;

            var reportData = new ExcelValues();

            #region HEADER TABELA
            reportData.Collumns.Add("Compareceu");
            reportData.Collumns.Add("Documento");
            reportData.Collumns.Add("Atividade");
            reportData.Collumns.Add("Data de Entrada");
            reportData.Collumns.Add("Data de Saída");
            reportData.Collumns.Add("Tempo de Atividade");
            reportData.Collumns.Add("Horas Cumpridas");
            reportData.Collumns.Add("Horas Remanescentes");
            #endregion

            #region DADOS DA TABELA

            foreach (var item in data.Frequencies)
            {
                reportData.Values.Add(new List<string>
                {
                    item.Appear ? "Sim": "Não",
                    item.PersonDocument,
                    item.Activity,
                    item.EntryTime.ToString("dd/MM/yyyy HH:mm"),
                    item.ExitTime.ToString("dd/MM/yyyy HH:mm"),
                    item.ActivityTotalTime.ToString(),
                    item.FulfilledHours.ToString(),
                    item.RemainingHours.ToString()
                });
            }
            #endregion

            return BlExcelWritter.GetExcel("Relatório de Frequência", reportData);
        }

        private BaseApiOutput BasicValidation(Frequency input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (input.Id != 0)
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
