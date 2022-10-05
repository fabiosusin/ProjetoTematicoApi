using DAO.DBConnection;
using DAO.Intra.PersonDAO;
using DTO.General.Base.Api.Output;
using DTO.Intra.Person.Database;
using DTO.Intra.Person.Input;
using DTO.Intra.Person.Output;
using System.Linq;
using Useful.Extensions;

namespace Business.API.Intra.BlPerson
{
    public class BlPerson
    {
        private readonly IntraPersonDAO IntraPersonDAO;

        public BlPerson(XDataDatabaseSettings settings)
        {
            IntraPersonDAO = new(settings);
        }

        public BaseApiOutput UpsertPerson(Person input)
        {
            var baseValidation = BasicValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            var result = string.IsNullOrEmpty(input.Id) ? IntraPersonDAO.Insert(input) : IntraPersonDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar o novo Pessoa!") : new(true);
        }

        public DTO.Intra.Person.Database.Person GetPerson(string cpfCnpj) => string.IsNullOrEmpty(cpfCnpj) ? null : IntraPersonDAO.FindOne(x => x.CpfCnpj == cpfCnpj);

        public BaseApiOutput DeletePerson(string id)
        {
            if (string.IsNullOrEmpty(id))
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
