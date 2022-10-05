using DAO.DBConnection.SqlServer;
using DTO.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.DBConnection
{
    internal class RepositorySqlServer<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected SqlServerContext Db;
        protected DbSet<TEntity> DbSet;

        internal RepositorySqlServer(IDBSettings settings)
        {
            Db = new(settings.ConnectionString);
            DbSet = Db.Set<TEntity>();

            Db.RunMigrations();
        }

        public virtual TEntity Insert(IBaseData obj)
        {
            var result = DbSet.Add((TEntity)obj).Entity;

            SaveChanges();
            return result;
        }

        public virtual TEntity InsertIfNotExist(IBaseData obj, Expression<Func<TEntity, bool>> predicate)
        {
            var exist = DbSet.Where(predicate);
            var entity = (TEntity)obj;
            if (exist?.Count() > 0)
                return entity;

            var result = DbSet.Add(entity).Entity;

            SaveChanges();
            return result;
        }

        public void Remove(IBaseData obj)
        {
            _ = DbSet.Remove((TEntity)obj);
            SaveChanges();
        }

        public virtual TEntity Update(IBaseData obj)
        {
            var result = (TEntity)obj;
            DbSet.Attach(result);
            Db.Entry(obj).State = EntityState.Modified;

            SaveChanges();
            return result;
        }


        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => DbSet.Where(predicate);

        public virtual TEntity FindOne() => DbSet.Take(1).FirstOrDefault();

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> predicate) => DbSet.Where(predicate).Take(1).FirstOrDefault();

        public virtual TEntity FindById(string id) => DbSet.Find(new Guid(id));

        public virtual IEnumerable<TEntity> FindAll() => DbSet.ToList();

        public virtual void RemoveById(string id)
        {
            DbSet.Remove(DbSet.Find(new Guid(id)));
            SaveChanges();
        }

        public int SaveChanges() => Db.SaveChanges();

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
