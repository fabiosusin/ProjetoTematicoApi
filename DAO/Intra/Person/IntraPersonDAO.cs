using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Person.Database;
using DTO.Intra.Person.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Intra.PersonDAO
{
    public class IntraPersonDAO : IBaseDAO<Person>
    {
        internal RepositoryMongo<Person> Repository;
        public IntraPersonDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(Person obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Person obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Person obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Person obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Person FindOne() => Repository.FindOne();

        public Person FindOne(Expression<Func<Person, bool>> predicate) => Repository.FindOne(predicate);

        public Person FindById(string id) => Repository.FindById(id);

        public IEnumerable<Person> Find(Expression<Func<Person, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<Person> FindAll() => Repository.FindAll();

        public long PersonsCount() => Repository.FindAll().Count();

        public IEnumerable<Person> List(PersonListInput input) => Repository.FindAll();

        private static IMongoQuery GenerateFilters(PersonFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<Person>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.CpfCnpj))
                queryList.Add(Query<Person>.Matches(x => x.CpfCnpj, $"(?i).*{string.Join(".*", Regex.Split(input.CpfCnpj, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
