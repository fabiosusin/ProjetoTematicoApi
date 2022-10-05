using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Employee.Database;
using DTO.Intra.Employee.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Intra.EmployeeDAO
{
    public class IntraEmployeeDAO : IBaseDAO<Employee>
    {
        internal RepositoryMongo<Employee> Repository;
        public IntraEmployeeDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(Employee obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(Employee obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(Employee obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(Employee obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public Employee FindOne() => Repository.FindOne();

        public Employee FindOne(Expression<Func<Employee, bool>> predicate) => Repository.FindOne(predicate);

        public Employee FindById(string id) => Repository.FindById(id);

        public IEnumerable<Employee> Find(Expression<Func<Employee, bool>> predicate) => Repository.Find(predicate);

        public IEnumerable<Employee> FindAll() => Repository.FindAll();

        public long EmployeesCount() => Repository.FindAll().Count();

        public IEnumerable<Employee> List(EmployeeListInput input) => Repository.FindAll();

        private static IMongoQuery GenerateFilters(EmployeeFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<Employee>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.CpfCnpj))
                queryList.Add(Query<Employee>.Matches(x => x.CpfCnpj, $"(?i).*{string.Join(".*", Regex.Split(input.CpfCnpj, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
