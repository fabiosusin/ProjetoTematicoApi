using DAO.DBConnection;
using DAO.Intra.SituationDAO;
using DTO.General.Base.Api.Output;
using DTO.Intra.Situation.Database;
using DTO.Intra.Situation.Output;
using System.Linq;

namespace Business.API.Intra.BlSituation
{
    public class BlSituation
    {
        private readonly SituationDAO SituationDAO;
        public BlSituation(XDataDatabaseSettings settings)
        {
            SituationDAO = new(settings);
        }

        public BaseApiOutput UpsertSituation(Situation input)
        {
            var baseValidation = BasicValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            var result = string.IsNullOrEmpty(input.Id) ? SituationDAO.Insert(input) : SituationDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar uma nova Situação Processual!") : new(true);
        }

        public Situation GetSituation(int processNumber) => processNumber == 0 ? null : SituationDAO.FindOne(x => x.ProcessNumber == processNumber);

        public BaseApiOutput DeleteSituation(string id)
        {
            if (string.IsNullOrEmpty(id))
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

            if (SituationDAO.FindOne(x => x.ProcessNumber == input.ProcessNumber && x.Id != input.Id) != null)
                return new("Já existe uma Situação Processual cadastrada com este Número de Processo");

            return new(true);
        }
    }
}
