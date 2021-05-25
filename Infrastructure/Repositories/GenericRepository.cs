using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Add(T entity)
        {
            this._context.Set<T>().Add(entity);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression, bool isNoTracking = false)
        {
            IQueryable<T> dbSet = this._context.Set<T>();
            if (isNoTracking)
            { 
                dbSet = dbSet.AsNoTracking();
            }
            return dbSet.Where(expression);
        }
        
        public IQueryable<T> All(bool isNoTracking = false)
        {
            Expression<Func<T, bool>> expression = x => true;
            return Find(expression, isNoTracking);
        }

        public void Remove(T entity)
        {
            this._context.Set<T>().Remove(entity);
        }
    }
}
