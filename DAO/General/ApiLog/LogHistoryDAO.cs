using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.General.Log.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.General.Log
{
    public class LogHistoryDAO : IBaseDAO<AppLogHistory>
    {
        internal RepositoryMongo<AppLogHistory> Repository;
        public LogHistoryDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(AppLogHistory obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(AppLogHistory obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(AppLogHistory obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(AppLogHistory obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public AppLogHistory FindOne() => Repository.FindOne();

        public AppLogHistory FindOne(Expression<Func<AppLogHistory, bool>> predicate) => Repository.FindOne(predicate);

        public AppLogHistory FindById(string id) => Repository.FindById(id);

        public IEnumerable<AppLogHistory> Find(Expression<Func<AppLogHistory, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<AppLogHistory> FindAll() => Repository.FindAll();
    }
}
