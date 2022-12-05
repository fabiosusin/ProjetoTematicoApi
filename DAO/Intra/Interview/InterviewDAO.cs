using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Interview.Database;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Intra.InterviewDAO
{
    public class InterviewDAO : IBaseDAO<Interview>
    {
        internal RepositoryMongo<Interview> Repository;
        public InterviewDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(Interview obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Interview obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Interview obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Interview obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Interview FindOne() => Repository.FindOne();

        public Interview FindOne(Expression<Func<Interview, bool>> predicate) => Repository.FindOne(predicate);

        public Interview FindById(string id) => Repository.FindById(id);

        public IEnumerable<Interview> Find(Expression<Func<Interview, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<Interview> FindAll() => Repository.FindAll();

        public IEnumerable<Interview> List() => Repository.Collection.FindAll();
    }
}
