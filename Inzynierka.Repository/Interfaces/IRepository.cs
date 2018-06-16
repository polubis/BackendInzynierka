using Inzynierka.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Inzynierka.Repository.Interfaces
{
    public interface IRepository<T> where T : Entity // Nie wiem
    {
        int Insert(T entity);
        bool Exist(Expression<Func<T, bool>> expression);
        T GetBy(Expression<Func<T, bool>> getBy, params Expression<Func<T, object>>[] includes);
    } 
}
