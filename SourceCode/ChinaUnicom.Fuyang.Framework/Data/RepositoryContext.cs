using System;
using System.Collections.Generic;
//using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ChinaUnicom.Fuyang.Framework.Data.Extensions;

namespace ChinaUnicom.Fuyang.Framework.Data
{
    public class RepositoryContext : IRepositoryContext
    {
        protected virtual DbContext Context { get; set; }

        public DbContext DbContext { get { return Context; } }

        public RepositoryContext()
        { }

        public bool IsCommitted { get; private set; }
        public int Commit(bool validateOnSaveEnabled = true)
        {
            if (IsCommitted)
                return 0;

            try
            {
                int result = Context.SaveChanges(validateOnSaveEnabled);
                IsCommitted = true;
                return result;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                { }

                return 0;
            }
        }
        public void Rollback()
        {
            IsCommitted = false;
        }

        public DbSet<T> Set<T>() where T : Entity
        {
            return Context.Set<T>();
        }

        public void RegisterNew<T>(T entity) where T : Entity
        {
            EntityState state = Context.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                Context.Entry(entity).State = EntityState.Added;
            }
            IsCommitted = false;
        }

        public void RegisterNew<T>(IEnumerable<T> entities) where T : Entity {
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (T entity in entities)
                {
                    RegisterNew<T>(entity);
                }
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public void RegisterModified<T>(T entity) where T : Entity
        {
            Context.Update<T>(entity);
            IsCommitted = false;
        }

        public void RegisterModified<T>(IEnumerable<T> entities) where T : Entity
        {            
            Context.Update<T>(entities.ToArray());

            IsCommitted = false;            
        }

        public void RegisterModified<T>(Expression<Func<T, object>> propertyExpression, T entity) where T:Entity
        {
            Context.Update<T>(propertyExpression, entity);
            IsCommitted = false;
        }

        public void RegisterDeleted<T>(T entity) where T : Entity
        {
            Context.Entry(entity).State = EntityState.Deleted;
            IsCommitted = false;
        }

        public void RegisterDeleted<T>(IEnumerable<T> entities) where T:Entity
        {
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (T entity in entities)
                {
                    RegisterDeleted<T>(entity);
                }
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public void Dispose()
        {
            if (!IsCommitted)
            {
                Commit();
            }

            Context.Dispose();
        }
    }
}
