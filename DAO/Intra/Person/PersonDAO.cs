using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Person.Database;
using DTO.Intra.Person.Input;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Useful.Extensions;

namespace DAO.Intra.PersonDAO
{
    public class PersonDAO : IBaseDAO<Person>
    {
        internal RepositorySqlServer<Person> Repository;
        public PersonDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.SqlServerSettings);

        public DAOActionResultOutput Insert(Person obj)
        {
            obj.Id = NumberExtension.RandomNumber(6);
            var result = Repository.Insert(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Person obj)
        {
            var result = Repository.Update(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Person obj) =>obj.Id == 0 ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Person obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(int id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Person FindOne() => Repository.FindOne();

        public Person FindOne(Expression<Func<Person, bool>> predicate) => Repository.FindOne(predicate);

        public Person FindById(int id) => Repository.FindById(id);

        public IEnumerable<Person> Find(Expression<Func<Person, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<Person> FindAll() => Repository.FindAll();

        public long PersonsCount() => Repository.FindAll().Count();

        public IEnumerable<Person> List(PersonListInput input) => input?.Filters == null ?
            FindAll() : string.IsNullOrEmpty(input.Filters.Name) && string.IsNullOrEmpty(input.Filters.CpfCnpj) ? FindAll() :
            !string.IsNullOrEmpty(input.Filters.Name) && !string.IsNullOrEmpty(input.Filters.CpfCnpj) ? Find(x => x.Name.Contains(input.Filters.Name) && x.CpfCnpj.Contains(input.Filters.CpfCnpj)) :
            !string.IsNullOrEmpty(input.Filters.CpfCnpj) ? Find(x => x.CpfCnpj.Contains(input.Filters.CpfCnpj)) : Find(x => x.Name.Contains(input.Filters.Name));

    }
}
