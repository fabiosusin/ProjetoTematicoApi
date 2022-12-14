using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.User.Database;
using DTO.Intra.User.Input;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            var result = Repository.Update(obj);
            if (result?.Id == 0)
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(AppUser obj) => obj.Id == 0 ? Insert(obj) : Update(obj);

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

        public IEnumerable<AppUser> List(UserListInput input) => input?.Filters == null ?
            FindAll() : string.IsNullOrEmpty(input.Filters.Name) && string.IsNullOrEmpty(input.Filters.Username) ? FindAll() :
            !string.IsNullOrEmpty(input.Filters.Name) && !string.IsNullOrEmpty(input.Filters.Username) ? Find(x => x.Name.Contains(input.Filters.Name) && x.Username.Contains(input.Filters.Username)) :
            !string.IsNullOrEmpty(input.Filters.Username) ? Find(x => x.Username.Contains(input.Filters.Username)) : Find(x => x.Name.Contains(input.Filters.Name));
    }
}
