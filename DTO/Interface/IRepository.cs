using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DTO.Interface
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Insert(IBaseData obj);
        void Remove(IBaseData obj);
        void RemoveById(Guid id);
        IEnumerable<TEntity> FindAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity FindById(Guid id);
        TEntity Update(IBaseData obj);
        int SaveChanges();
    }
}
