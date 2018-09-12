using Inzynierka.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inzynierka.Repository.Interfaces
{
    public interface IRepository<T> where T : Entity // Nie wiem
    {
        Task<int> Insert(T entity);
        bool Exist(Expression<Func<T, bool>> expression);
        T GetBy(Expression<Func<T, bool>> getBy, params Expression<Func<T, object>>[] includes);
        int Update(T entity);
        IQueryable<T> GetAllBy(Expression<Func<T, bool>> getBy, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);

        Task<T> InsertAndReturnObject(T entity);
        T GetByWithRelatedInclude(Expression<Func<T, bool>> getBy, Expression<Func<T, object>> include,
           Expression<Func<object, object>> thenInclude);
        int Delete(Expression<Func<T, bool>> expression);
    }
}
