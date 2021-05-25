using System;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> All(bool isNoTracking = false);
        IQueryable<T> Find(Expression<Func<T, bool>> expression, bool isNoTracking = false);
        void Add(T entity);
        void Remove(T entity);
    }
}
