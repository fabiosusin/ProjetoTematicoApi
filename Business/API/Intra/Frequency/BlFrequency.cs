using DAO.DBConnection;
using DAO.Intra.FrequencyDAO;
using DTO.General.Base.Api.Output;
using DTO.Intra.Frequency.Output;
using DTO.Intra.FrequencyDB.Database;
using System.Linq;

namespace Business.API.Intra.BlFrequency
{
    public class BlFrequency
    {
        private readonly FrequencyDAO FrequencyDAO;
        public BlFrequency(XDataDatabaseSettings settings)
        {
            FrequencyDAO = new(settings);
        }

        public BaseApiOutput UpsertFrequency(Frequency input)
        {
            var baseValidation = BasicValidation(input);
            if (!baseValidation.Success)
                return baseValidation;

            var result = string.IsNullOrEmpty(input.Id) ? FrequencyDAO.Insert(input) : FrequencyDAO.Update(input);
            return result == null ? new("Não foi possível cadastrar uma nova Frequência!") : new(true);
        }

        public Frequency GetFrequency(string id) => string.IsNullOrEmpty(id) ? null : FrequencyDAO.FindOne(x => x.Id == id);

        public BaseApiOutput DeleteFrequency(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Requisição mal formada!");

            var Frequency = FrequencyDAO.FindById(id);
            if (Frequency == null)
                return new("Parceiro não encontrado!");

            FrequencyDAO.Remove(Frequency);
            return new(true);
        }

        public FrequencyListOutput List()
        {
            var result = FrequencyDAO.List();
            if (!(result?.Any() ?? false))
                return new("Nenhuma Frequência encontrada!");

            return new(result);
        }

        private BaseApiOutput BasicValidation(Frequency input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            return new(true);
        }
    }
}
