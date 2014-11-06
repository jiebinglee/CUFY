using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaUnicom.Fuyang.Framework.Data
{
    public class Pageable<T>
    {
        private IQueryable<T> _source;
        private int _pageNumber;
        private int _pageSize;

        private int _totalCount;
        private int _totalPages;        
        private int _pageCount;

        private List<T> _pageResult;

        public Pageable(IQueryable<T> source, Action<Orderable<T>> order, int pageNumber, int pageSize)
        {
            _source = source;

            _pageNumber = pageNumber;
            _pageSize = pageSize;

            _totalCount = source.ToList().Count;
            _totalPages = (int)Math.Ceiling(_totalCount / (double)_pageSize);

            var orderable = new Orderable<T>(source);
            order(orderable);
            _pageResult = orderable.Queryable.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();

            _pageCount = _pageResult.ToList().Count;
        }

        public List<T> Source
        {
            get
            {
                return _source.ToList();
            }
        }

        public int TotalPages
        {
            get 
            { 
                return _totalPages; 
            } 
        }

        public int TotalCount
        {
            get
            {
                return _totalCount;
            }
        }

        public int PageCount
        {
            get
            {
                return _pageCount;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
        }

        public int PageNumber
        {
            get
            {
                return _pageNumber;
            }
        }

        public List<T> PageResult
        {
            get
            {
                return _pageResult;
            }
        }

        //private IQueryable<T> DataPaging<T, TKey>(IQueryable<T> source, Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize)
        //{
        //    string sortingDir = "OrderBy";
        //    string sortExpression = "ChannelId";
        //    ParameterExpression param = Expression.Parameter(typeof(T), sortExpression);
        //    PropertyInfo pi = typeof(T).GetProperty(sortExpression);
        //    Type[] types = new Type[2];
        //    types[0] = typeof(T);
        //    types[1] = pi.PropertyType;
        //    Expression expr = Expression.Call(typeof(Queryable), sortingDir, types, source.Expression, Expression.Lambda(Expression.Property(param, sortExpression), param));
        //    IQueryable<T> query = source.AsQueryable().Provider.CreateQuery<T>(expr);


        //    return source.OrderBy(keySelector).Skip(pageNumber * pageSize).Take(pageSize);
        //}
    }
}
