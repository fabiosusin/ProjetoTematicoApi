using DAO.DBConnection;
using DAO.Intra.CompanyDao;
using DTO.General.Base.Api.Output;
using DTO.Intra.Company.Database;
using DTO.Intra.Company.Input;
using DTO.Intra.Company.Output;
using System.Linq;
using Useful.Extensions;

namespace Business.API.Intra.BlCompany
{
    public class BlCompany
    {
        private readonly IntraCompanyDAO IntraCompanyDAO;
        public BlCompany(XDataDatabaseSettings settings)
        {
            IntraCompanyDAO = new(settings);
        }

        public BaseApiOutput UpsertCompany(Company input)
        {
            var baseValidation = BasicValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            var result = string.IsNullOrEmpty(input.Id) ? IntraCompanyDAO.Insert(input) : IntraCompanyDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar um novo Parceiro!") : new(true);
        }

        public Company GetCompany(string cpfCnpj) => string.IsNullOrEmpty(cpfCnpj) ? null : IntraCompanyDAO.FindOne(x => x.Cnpj == cpfCnpj);

        public BaseApiOutput DeleteCompany(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Requisição mal formada!");

            var Company = IntraCompanyDAO.FindById(id);
            if (Company == null)
                return new("Parceiro não encontrado!");

            IntraCompanyDAO.Remove(Company);
            return new(true);
        }

        public CompanyListOutput List(CompanyListInput input)
        {
            var result = IntraCompanyDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Parceiro encontrado!");

            return new(result);
        }

        private BaseApiOutput BasicValidation(Company input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.Name))
                return new("Informe o Nome!");

            if (!input.Cnpj?.IsCnpj() ?? false)
                return new("Informe um CNPJ válido!");

            if (IntraCompanyDAO.FindOne(x => x.Cnpj == input.Cnpj && x.Id != input.Id) != null)
                return new("Já existe um Parceiro cadastrado com este CNPJ");

            return new(true);
        }
    }
}
