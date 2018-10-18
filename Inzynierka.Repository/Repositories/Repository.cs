using Inzynierka.Data.DbModels;
using Inzynierka.Repository.AppDbContext;
using Inzynierka.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<int> InsertList(List<T> entities)
        {
            if(entities.Count == 0)
            {
                throw new ArgumentNullException("entity");
            }

            await _dbContext.AddRangeAsync(entities);

            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            
            entity.ModifiedDate = entity.CreationDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<T> InsertAndReturnObject(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entity.ModifiedDate = entity.CreationDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public bool Exist(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Any(expression);
        }

        public T GetByWithRelatedInclude(Expression<Func<T, bool>> getBy, Expression<Func<T, object>> include, 
            Expression<Func<object, object>> thenInclude)
        {
            IQueryable<T> query = _dbSet;

            return query.Include(include).ThenInclude(thenInclude).FirstOrDefault(getBy);
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

        public IQueryable<T> GetAllBy(Expression<Func<T, bool>> getBy, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach(var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(getBy);
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
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

        public async Task LoadRelatedCollectionThenIncludeCollection<TInclude, TIncluded, TThenInclude>(T entity, Expression<Func<T, IEnumerable<TInclude>>> collection,
            Expression<Func<TInclude, TIncluded>> include, Expression<Func<TIncluded, IEnumerable<TThenInclude>>> thenInclude)
            where TInclude : Entity where TThenInclude : Entity where TIncluded : Entity
        {
            await _dbContext.Entry(entity).Collection(collection).Query().Include(include).ThenInclude(thenInclude).LoadAsync();

        }

        public int Delete(Expression<Func<T, bool>> expression)
        {
            var entity = _dbSet.SingleOrDefault(expression);
            if (entity == null)
            {
                throw new NullReferenceException();
            }
            _dbSet.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<T> GetAllByWithLimit(Expression<Func<T, bool>> getBy, Expression<Func<T, object>> orderByDescending,
            Expression<Func<T, object>> orderByAscending,
            int limit, int skip, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if(getBy != null)
                query = query.Where(getBy);
            
            if(orderByAscending != null)
                query = query.OrderBy(orderByAscending);

            if (orderByDescending != null)
                query = query.OrderByDescending(orderByDescending);

            if(query.Count() < limit)
            {
                return query;
            }

            return query.Skip((skip-1) * limit).Take(limit);
        }


    }
}
