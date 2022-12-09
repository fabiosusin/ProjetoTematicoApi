using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Frequency.Input;
using DTO.Intra.FrequencyDB.Database;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Useful.Extensions;

namespace DAO.Intra.FrequencyDAO
{
    public class FrequencyDAO : IBaseDAO<Frequency>
    {
        internal RepositorySqlServer<Frequency> Repository;
        public FrequencyDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.SqlServerSettings);

        public DAOActionResultOutput Insert(Frequency obj)
        {
            obj.Id = NumberExtension.RandomNumber(6);
            var result = Repository.Insert(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Frequency obj)
        {
            var result = Repository.Update(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Frequency obj) =>obj.Id == 0 ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Frequency obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(int id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Frequency FindOne() => Repository.FindOne();

        public Frequency FindOne(Expression<Func<Frequency, bool>> predicate) => Repository.FindOne(predicate);

        public Frequency FindById(int id) => Repository.FindById(id);

        public IEnumerable<Frequency> Find(Expression<Func<Frequency, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<Frequency> FindAll() => Repository.FindAll();

        public IEnumerable<Frequency> List(FrequencyListInput input) => input?.Filters == null ?
            FindAll() : string.IsNullOrEmpty(input.Filters.Activity) && string.IsNullOrEmpty(input.Filters.CpfCnpj) ? FindAll() :
            !string.IsNullOrEmpty(input.Filters.Activity) && !string.IsNullOrEmpty(input.Filters.CpfCnpj) ? Find(x => x.Activity.Contains(input.Filters.Activity) && x.PersonDocument.Contains(input.Filters.CpfCnpj)) :
            !string.IsNullOrEmpty(input.Filters.CpfCnpj) ? Find(x => x.PersonDocument.Contains(input.Filters.CpfCnpj)) : Find(x => x.Activity.Contains(input.Filters.Activity));
    }
}
