using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.General.Log.Database;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Useful.Extensions;

namespace DAO.General.Log
{
    public class LogHistoryDAO : IBaseDAO<AppLogHistory>
    {
        internal RepositorySqlServer<AppLogHistory> Repository;
        public LogHistoryDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.SqlServerSettings);

        public DAOActionResultOutput Insert(AppLogHistory obj)
        {
            obj.Id = NumberExtension.RandomNumber(6);
            var result = Repository.Insert(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(AppLogHistory obj)
        {
            var result = Repository.Update(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(AppLogHistory obj) =>obj.Id == 0 ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(AppLogHistory obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(int id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public AppLogHistory FindOne() => Repository.FindOne();

        public AppLogHistory FindOne(Expression<Func<AppLogHistory, bool>> predicate) => Repository.FindOne(predicate);

        public AppLogHistory FindById(int id) => Repository.FindById(id);

        public IEnumerable<AppLogHistory> Find(Expression<Func<AppLogHistory, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<AppLogHistory> FindAll() => Repository.FindAll();
    }
}
