using DAO.DBConnection;
using DAO.Intra.InterviewDAO;
using DAO.Intra.PersonDAO;
using DTO.General.Base.Api.Output;
using DTO.Intra.Interview.Database;
using DTO.Intra.Interview.Output;
using System.Linq;

namespace Business.API.Intra.BlInterview
{
    public class BlInterview
    {
        private readonly InterviewDAO InterviewDAO;
        private readonly IntraPersonDAO IntraPersonDAO;
        public BlInterview(XDataDatabaseSettings settings)
        {
            InterviewDAO = new(settings);
            IntraPersonDAO = new(settings);
        }

        public BaseApiOutput UpsertInterview(Interview input)
        {
            input.PersonId = IntraPersonDAO.FindOne(x => x.CpfCnpj == input.PersonDocument)?.Id;
            var baseValidation = BasicValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            var result = string.IsNullOrEmpty(input.Id) ? InterviewDAO.Insert(input) : InterviewDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar uma nova Entrevista!") : new(true);
        }

        public Interview GetInterview(string id) => string.IsNullOrEmpty(id) ? null : InterviewDAO.FindOne(x => x.Id == id);

        public BaseApiOutput DeleteInterview(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Requisição mal formada!");

            var Interview = InterviewDAO.FindById(id);
            if (Interview == null)
                return new("Entravista não encontrada!");

            InterviewDAO.Remove(Interview);
            return new(true);
        }

        public InterviewListOutput List()
        {
            var result = InterviewDAO.List();
            if (!(result?.Any() ?? false))
                return new("Nenhuma Entravista encontrada!");

            return new(result);
        }

        private BaseApiOutput BasicValidation(Interview input)
        {
            if (input == null)
                return new("Requisição mal formada!");
           
            if (string.IsNullOrEmpty(input.PersonDocument))
                return new("Documento da Pessoa não informado!");

            if (IntraPersonDAO.FindOne(x => x.CpfCnpj == input.PersonDocument) == null)
                return new("Pessoa não cadastrada no sistema!");

            if (InterviewDAO.FindOne(x => x.PersonId == input.PersonId && x.Id != input.Id) != null)
                return new("Pessoa já está vinculada a uma Situação Processual!");

            return new(true);
        }
    }
}
