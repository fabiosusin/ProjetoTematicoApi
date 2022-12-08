using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Situation.Database;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Intra.SituationDAO
{
    public class SituationDAO : IBaseDAO<Situation>
    {
        internal RepositorySqlServer<Situation> Repository;
        public SituationDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.SqlServerSettings);

        public DAOActionResultOutput Insert(Situation obj)
        {
            var result = Repository.Insert(obj);
            if (result?.Id == Guid.Empty)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Situation obj)
        {
            var result = Repository.Update(obj);
            if (result?.Id == Guid.Empty)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Situation obj) => obj.Id == Guid.Empty ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Situation obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(Guid id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Situation FindOne() => Repository.FindOne();

        public Situation FindOne(Expression<Func<Situation, bool>> predicate) => Repository.FindOne(predicate);

        public Situation FindById(Guid id) => Repository.FindById(id);

        public IEnumerable<Situation> Find(Expression<Func<Situation, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<Situation> FindAll() => Repository.FindAll();

        public IEnumerable<Situation> List() => Repository.FindAll();
    }
}
