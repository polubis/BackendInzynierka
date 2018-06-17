using Inzynierka.Data.DbModels;
using Inzynierka.Repository.AppDbContext;
using Inzynierka.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace Inzynierka.Repository.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly ApplicationDbContext _dbContext;
        private DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }
        public int Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            
            entity.ModifiedDate = entity.CreationDate = DateTime.Now;
            _dbSet.Add(entity);
            return _dbContext.SaveChanges();
        }

        public bool Exist(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Any(expression);
        }

        public T GetBy(Expression<Func<T, bool>> getBy, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault(getBy);
        }

        public int Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);
            return _dbContext.SaveChanges();
        }

       
    }
}
