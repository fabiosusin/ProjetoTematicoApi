using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Situation.Database;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Useful.Extensions;

namespace DAO.Intra.SituationDAO
{
    public class SituationDAO : IBaseDAO<Situation>
    {
        internal RepositorySqlServer<Situation> Repository;
        public SituationDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.SqlServerSettings);

        public DAOActionResultOutput Insert(Situation obj)
        {
            obj.Id = NumberExtension.RandomNumber(6);
            var result = Repository.Insert(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Situation obj)
        {
            var result = Repository.Update(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Situation obj) =>obj.Id == 0 ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Situation obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(int id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Situation FindOne() => Repository.FindOne();

        public Situation FindOne(Expression<Func<Situation, bool>> predicate) => Repository.FindOne(predicate);

        public Situation FindById(int id) => Repository.FindById(id);

        public IEnumerable<Situation> Find(Expression<Func<Situation, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<Situation> FindAll() => Repository.FindAll();

        public IEnumerable<Situation> List() => Repository.FindAll();
    }
}
