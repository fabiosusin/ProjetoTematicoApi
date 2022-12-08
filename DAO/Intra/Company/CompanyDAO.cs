using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Company.Database;
using DTO.Intra.Company.Input;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Useful.Extensions;

namespace DAO.Intra.CompanyDao
{
    public class CompanyDAO : IBaseDAO<Company>
    {
        internal RepositorySqlServer<Company> Repository;
        public CompanyDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.SqlServerSettings);

        public DAOActionResultOutput Insert(Company obj)
        {
            obj.Id = NumberExtension.RandomNumber(6);
            var result = Repository.Insert(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Company obj)
        {
            var result = Repository.Update(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Company obj) =>obj.Id == 0 ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Company obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(int id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Company FindOne() => Repository.FindOne();

        public Company FindOne(Expression<Func<Company, bool>> predicate) => Repository.FindOne(predicate);

        public Company FindById(int id) => Repository.FindById(id);

        public IEnumerable<Company> Find(Expression<Func<Company, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<Company> FindAll() => Repository.FindAll();

        public long CompanysCount() => Repository.FindAll().Count();

        public IEnumerable<Company> List(CompanyListInput input) => FindAll();

        //public IEnumerable<Company> List(CompanyListInput input) => input == null ?
        //    Repository.Collection.FindAll(): input.Paginator == null ?
        //    Repository.Collection.Find(GenerateFilters(input.Filters)): Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

        private static IMongoQuery GenerateFilters(CompanyFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<Company>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.Cnpj))
                queryList.Add(Query<Company>.Matches(x => x.Cnpj, $"(?i).*{string.Join(".*", Regex.Split(input.Cnpj, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}