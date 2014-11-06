using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ChinaUnicom.Fuyang.Framework.Data
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        IUnitOfWork UnitOfWork { get; }

        T Create(T entity, bool isSave = true);
        IEnumerable<T> Create(IEnumerable<T> entities, bool isSave = true);

        int Update(T entity, bool isSave = true);
        int Update(IEnumerable<T> entities, bool isSave = true);
        int Update(Expression<Func<T, object>> propertyExpression, T entity, bool isSave = true);

        int Delete(int id, bool isSave = true);
        int Delete(T entity, bool isSave = true);
        int Delete(IEnumerable<T> entities, bool isSave = true);
        int Delete(Expression<Func<T, bool>> predicate, bool isSave = true);

        void Copy(T source, T target);

        int Flush();

        T Get(int id);
        T Get(Expression<Func<T, bool>> predicate);

        IQueryable<T> Table { get; }

        int Count(Expression<Func<T, bool>> predicate);
        IQueryable<T> Fetch(Expression<Func<T, bool>> predicate);
        IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order);
        IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip, int count);
    }
}
