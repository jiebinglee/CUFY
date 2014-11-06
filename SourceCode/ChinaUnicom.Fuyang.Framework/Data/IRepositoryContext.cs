using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ChinaUnicom.Fuyang.Framework.Data
{
    public interface IRepositoryContext : IUnitOfWork
    {
        DbContext DbContext { get; }

        DbSet<T> Set<T>() where T : Entity;

        void RegisterNew<T>(T entity) where T : Entity;
        void RegisterNew<T>(IEnumerable<T> entities) where T : Entity;

        void RegisterModified<T>(T entity) where T : Entity;
        void RegisterModified<T>(IEnumerable<T> entities) where T : Entity;
        void RegisterModified<T>(Expression<Func<T, object>> propertyExpression, T entity) where T : Entity;

        void RegisterDeleted<T>(T entity) where T : Entity;
        void RegisterDeleted<T>(IEnumerable<T> entities) where T : Entity;
    }
}
