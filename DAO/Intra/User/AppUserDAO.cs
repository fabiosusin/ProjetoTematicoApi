using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.User.Database;
using DTO.Intra.User.Input;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Useful.Extensions;

namespace DAO.Intra.UserDAO
{
    public class AppUserDAO : IBaseDAO<AppUser>
    {
        internal RepositorySqlServer<AppUser> Repository;
        public AppUserDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.SqlServerSettings);

        public DAOActionResultOutput Insert(AppUser obj)
        {
            obj.Id = NumberExtension.RandomNumber(6);
            var result = Repository.Insert(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(AppUser obj)
        {
            if (string.IsNullOrEmpty(obj.Password))
                obj.Password = FindById(obj.Id)?.Password;

            var result = Repository.Update(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(AppUser obj) =>obj.Id == 0 ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(AppUser obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(int id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public AppUser FindOne() => Repository.FindOne();

        public AppUser FindOne(Expression<Func<AppUser, bool>> predicate) => Repository.FindOne(predicate);

        public AppUser FindById(int id) => Repository.FindById(id);

        public IEnumerable<AppUser> Find(Expression<Func<AppUser, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<AppUser> FindAll() => Repository.FindAll();

        public IEnumerable<AppUser> List(UserListInput input) => input == null ?
            FindAll() : input.Paginator == null ? FindAll() : FindAll();
            //Repository.Collection.Find(GenerateFilters(input.Filters)) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

        private static IMongoQuery GenerateFilters(UserFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<AppUser>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.Username))
                queryList.Add(Query<AppUser>.Matches(x => x.Username, $"(?i).*{string.Join(".*", Regex.Split(input.Username, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
