using DAO.DBConnection;
using DAO.Intra.EmployeeDAO;
using DTO.General.Base.Api.Output;
using DTO.Intra.Employee.Input;
using DTO.Intra.Employee.Output;
using System.Linq;
using Useful.Extensions;

namespace Business.API.Intra.Employee
{
    public class BlEmployee
    {
        private readonly IntraEmployeeDAO IntraEmployeeDAO;

        public BlEmployee(XDataDatabaseSettings settings)
        {
            IntraEmployeeDAO = new(settings);
        }

        public BaseApiOutput UpsertEmployee(DTO.Intra.Employee.Database.Employee input)
        {
            var baseValidation = BasicValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            var result = string.IsNullOrEmpty(input.Id) ? IntraEmployeeDAO.Insert(input) : IntraEmployeeDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar o novo Jogador!") : new(true);
        }

        public DTO.Intra.Employee.Database.Employee GetEmployee(string cpfCnpj) => string.IsNullOrEmpty(cpfCnpj) ? null : IntraEmployeeDAO.FindOne(x => x.CpfCnpj == cpfCnpj);

        public BaseApiOutput DeleteEmployee(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Requisição mal formada!");

            var employee = IntraEmployeeDAO.FindById(id);
            if (employee == null)
                return new("Jogador não encontrado!");

            IntraEmployeeDAO.Remove(employee);
            return new(true);
        }

        public EmployeeListOutput List(EmployeeListInput input)
        {
            var result = IntraEmployeeDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Jogador encontrado!");

            return new(result);
        }

        private BaseApiOutput BasicValidation(DTO.Intra.Employee.Database.Employee input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.Name))
                return new("Informe o Nome!");

            if (string.IsNullOrEmpty(input.CpfCnpj))
                return new("Informe o Cpf!");

            if (!input.CpfCnpj.IsCnpjOrCpf())
                return new("Cpf inválido!");

            if (IntraEmployeeDAO.FindOne(x => x.CpfCnpj == input.CpfCnpj && x.Id != input.Id) != null)
                return new("Já existe um Jogador cadastrado com este CPF");

            return new(true);
        }
    }
}
