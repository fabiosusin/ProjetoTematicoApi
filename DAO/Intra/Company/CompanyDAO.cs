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

        public IEnumerable<Company> List(CompanyListInput input) => input?.Filters == null ?
            FindAll() : string.IsNullOrEmpty(input.Filters.Name) && string.IsNullOrEmpty(input.Filters.Cnpj) ? FindAll() :
            !string.IsNullOrEmpty(input.Filters.Name) && !string.IsNullOrEmpty(input.Filters.Cnpj) ? Find(x => x.Name.Contains(input.Filters.Name) && x.Cnpj.Contains(input.Filters.Cnpj)) :
            !string.IsNullOrEmpty(input.Filters.Cnpj) ? Find(x => x.Cnpj.Contains(input.Filters.Cnpj)) : Find(x => x.Name.Contains(input.Filters.Name));

    }
}