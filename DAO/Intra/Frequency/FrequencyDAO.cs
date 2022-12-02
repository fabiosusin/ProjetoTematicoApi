using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.FrequencyDB.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Intra.FrequencyDAO
{
    internal class FrequencyDAO : IBaseDAO<Frequency>
    {
        internal RepositoryMongo<Frequency> Repository;
        public FrequencyDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(Frequency obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Frequency obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Frequency obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Frequency obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Frequency FindOne() => Repository.FindOne();

        public Frequency FindOne(Expression<Func<Frequency, bool>> predicate) => Repository.FindOne(predicate);

        public Frequency FindById(string id) => Repository.FindById(id);

        public IEnumerable<Frequency> Find(Expression<Func<Frequency, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<Frequency> FindAll() => Repository.FindAll();

        public long FrequencysCount() => Repository.FindAll().Count();

        public IEnumerable<Frequency> List() => Repository.Collection.FindAll()
    }
}
