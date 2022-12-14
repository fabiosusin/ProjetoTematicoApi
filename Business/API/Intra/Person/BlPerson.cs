using Business.General;
using DAO.DBConnection;
using DAO.Intra.PersonDAO;
using DTO.General.Api.Input;
using DTO.General.Api.Output;
using DTO.General.Base.Api.Output;
using DTO.Intra.Person.Database;
using DTO.Intra.Person.Input;
using DTO.Intra.Person.Output;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Useful.Extensions;

namespace Business.API.Intra.BlPerson
{
    public class BlPerson
    {
        private readonly PersonDAO IntraPersonDAO;

        public BlPerson(XDataDatabaseSettings settings)
        {
            IntraPersonDAO = new(settings);
        }

        public BaseApiOutput UpsertPerson(Person input)
        {
            var baseValidation = BasicValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            var result = input.Id == 0 ? IntraPersonDAO.Insert(input) : IntraPersonDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar o novo Pessoa!") : new(true);
        }

        public Person GetPerson(string cpfCnpj) => string.IsNullOrEmpty(cpfCnpj) ? null : IntraPersonDAO.FindOne(x => x.CpfCnpj == cpfCnpj);

        public BaseApiOutput DeletePerson(int id)
        {
            if (id == 0)
                return new("Requisição mal formada!");

            var Person = IntraPersonDAO.FindById(id);
            if (Person == null)
                return new("Pessoa não encontrado!");

            IntraPersonDAO.Remove(Person);
            return new(true);
        }

        public PersonListOutput List(PersonListInput input)
        {
            var result = IntraPersonDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Pessoa encontrado!");

            return new(result);
        }

        public GenerateDocOutput Export()
        {
            var data = IntraPersonDAO.List(null);
            if (!(data?.Any() ?? false))
                return null;

            var result = new List<Person>();
            foreach (var item in data)
                result.Add(CryptographyService.EncryptPerson(item));

            string json = JsonConvert.SerializeObject(result);
            var path = "C:\\person.json";
            File.WriteAllText(path, json);

            return new("Exportação_Prestador", path);
        }

        public BaseApiOutput Import(ImportFileInput input)
        {
            if (string.IsNullOrEmpty(input?.DataBase64))
                return new("Dados inválidos!");

            try
            {
                var data = new Regex("data:application/json;base64,").Replace(input.DataBase64, "");
                var byteArray = Convert.FromBase64String(data);
                var frequencies = JsonConvert.DeserializeObject<List<Person>>(Encoding.UTF8.GetString(byteArray));
                foreach (var frequency in frequencies)
                    IntraPersonDAO.Upsert(CryptographyService.DecryptPerson(frequency));

            }
            catch { return new("Dados em formato inválido!"); }

            return new(true);
        }

        private BaseApiOutput BasicValidation(Person input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.Name))
                return new("Informe o Nome!");

            if (string.IsNullOrEmpty(input.CpfCnpj))
                return new("Informe o Cpf!");

            if (!input.CpfCnpj.IsCnpjOrCpf())
                return new("Cpf inválido!");

            if (IntraPersonDAO.FindOne(x => x.CpfCnpj == input.CpfCnpj && x.Id != input.Id) != null)
                return new("Já existe um Pessoa cadastrado com este CPF");

            return new(true);
        }
    }
}
