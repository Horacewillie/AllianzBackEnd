using AllianzBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Infrastructure
{
    public static class Pagination
    {
        public static (IQueryable<T> dbSet, PagingData pagingData) Paginate<T, TKey>(this IQueryable<T> dbSet, Expression<Func<T, TKey>> orderByKey, int page = 1, int pageSize = 10, bool orderByDescending = false)
           where T : class
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;
            var totalPages = (int)Math.Ceiling(dbSet.Count() / (double)pageSize);

            var queryCount = dbSet.Count();

            IQueryable<T> query = orderByDescending ?
                dbSet.OrderByDescending(orderByKey) : dbSet.OrderBy(orderByKey);

            var pagingData = new PagingData
            {
                TotalCount = queryCount,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
            };

            return (query.Skip((page - 1) * pageSize).Take(pageSize), pagingData);
        }
    }
}
