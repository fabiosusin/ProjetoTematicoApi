using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Interview.Database;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Useful.Extensions;

namespace DAO.Intra.InterviewDAO
{
    public class InterviewDAO : IBaseDAO<Interview>
    {
        internal RepositorySqlServer<Interview> Repository;
        public InterviewDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.SqlServerSettings);

        public DAOActionResultOutput Insert(Interview obj)
        {
            obj.Id = NumberExtension.RandomNumber(6);
            var result = Repository.Insert(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Interview obj)
        {
            var result = Repository.Update(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Interview obj) =>obj.Id == 0 ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Interview obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(int id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Interview FindOne() => Repository.FindOne();

        public Interview FindOne(Expression<Func<Interview, bool>> predicate) => Repository.FindOne(predicate);

        public Interview FindById(int id) => Repository.FindById(id);

        public IEnumerable<Interview> Find(Expression<Func<Interview, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<Interview> FindAll() => Repository.FindAll();

        public IEnumerable<Interview> List(string document) => !string.IsNullOrEmpty(document) ? Repository.Find(x => x.PersonDocument.Contains(document)) : Repository.FindAll();
    }
}
