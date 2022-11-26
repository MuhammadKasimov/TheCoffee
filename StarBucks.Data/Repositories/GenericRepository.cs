﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarBucks.Data.Contexts;
using StarBucks.Data.IRepositories;

namespace StarBucks.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly StarbucksDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public GenericRepository(StarbucksDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public async ValueTask<T> CreateAsync(T entity) =>
            (await dbContext.AddAsync(entity)).Entity;

        public async ValueTask<bool> DeleteAsync(Expression<Func<T, bool>> expression)
        {
            var entity = await GetAsync(expression);

            if (entity == null)
                return false;

            dbSet.Remove(entity);

            return true;
        }

        public IQueryable<T> GetAll(
            Expression<Func<T, bool>> expression,
            string[] includes = null,
            bool isTracking = true)
        {
            IQueryable<T> query = expression is null ? dbSet : dbSet.Where(expression);

            if (includes != null)
                foreach (var include in includes)
                    if (!string.IsNullOrEmpty(include))
                        query = query.Include(include);

            if (!isTracking)
                query = query.AsNoTracking();

            return query;
        }

        public async ValueTask<T> GetAsync(Expression<Func<T, bool>> expression, string[] includes = null) =>
            await GetAll(expression, includes).FirstOrDefaultAsync();

        public T Update(T entity) =>
            dbSet.Update(entity).Entity;
    }
}
