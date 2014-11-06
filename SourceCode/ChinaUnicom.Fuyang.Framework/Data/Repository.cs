using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ChinaUnicom.Fuyang.Framework.Data
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly IRepositoryContext _unitOfWork;

        public Repository(IRepositoryContext unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        public virtual T Create(T entity, bool isSave = true)
        {
            _unitOfWork.RegisterNew<T>(entity);

            if (isSave)
            {
                if (_unitOfWork.Commit() != 0)
                    return entity;
                else
                    return null;
            }
            else
            {
                return entity;
            }
        }

        public virtual IEnumerable<T> Create(IEnumerable<T> entities, bool isSave = true)
        {
            _unitOfWork.RegisterNew<T>(entities);

            if (isSave)
            {
                if (_unitOfWork.Commit() != 0)
                    return entities;
                else
                    return null;
            }
            else
            {
                return entities;
            }
        }

        public virtual int Update(T entity, bool isSave = true)
        {
            _unitOfWork.RegisterModified<T>(entity);

            return isSave ? _unitOfWork.Commit() : 0;
        }

        public virtual int Update(IEnumerable<T> entities, bool isSave = true)
        {
            _unitOfWork.RegisterModified<T>(entities);

            return isSave ? _unitOfWork.Commit() : 0;
        }

        public virtual int Update(Expression<Func<T, object>> propertyExpression, T entity, bool isSave = true)
        {
            _unitOfWork.RegisterModified<T>(propertyExpression, entity);

            if (isSave)
            {
                var dbSet = _unitOfWork.Set<T>();
                dbSet.Local.Clear();
                return _unitOfWork.Commit(false);
            }
            return 0;
        }

        public virtual int Delete(int id, bool isSave = true)
        {
            T entity = _unitOfWork.Set<T>().Find(id);
            return entity != null ? Delete(entity, isSave) : 0;
        }

        public virtual int Delete(T entity, bool isSave = true)
        {
            _unitOfWork.RegisterDeleted<T>(entity);

            return isSave ? _unitOfWork.Commit() : 0;
        }        

        public virtual int Delete(IEnumerable<T> entities, bool isSave = true)
        {
            _unitOfWork.RegisterDeleted<T>(entities);

            return isSave ? _unitOfWork.Commit() : 0;
        }

        public virtual int Delete(Expression<Func<T, bool>> predicate, bool isSave = true)
        {
            List<T> entities = _unitOfWork.Set<T>().Where(predicate).ToList();

            return entities.Count() > 0 ? Delete(entities, isSave) : 0;
        }

        public virtual void Copy(T source, T target)
        {

        }

        public virtual int Flush()
        {
            return 0;
        }

        public virtual T Get(int id)
        {
            if (id != 0)
                return _unitOfWork.Set<T>().Find(id);
            else
                return null;
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).FirstOrDefault();
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return _unitOfWork.Set<T>();
            }
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).Count();
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate);
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            var orderable = new Orderable<T>(Fetch(predicate));
            order(orderable);
            return orderable.Queryable;
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip, int count)
        {
            return Fetch(predicate, order).Skip(skip).Take(count);
        }

        public void Dispose()
        {
            if (_unitOfWork != null)
                _unitOfWork.Dispose();
        }
    }
}
