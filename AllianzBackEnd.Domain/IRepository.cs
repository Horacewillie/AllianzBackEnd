using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Domain
{
    public interface IRepository<T>
        where T : DbEntityBase
    {

            /// <summary>
            /// Find an entity with a given set of keys
            /// </summary>
            /// <param name="keys"></param>
            /// <returns>A matching T instance or null</returns>
            Task<T> Find(params object[] keys);

            Task Rollback(CancellationToken cancellationToken = default);

            T Add(T entity);
            void Update(T entity);
            Task BeginTransaction(CancellationToken cancellationToken = default);
            Task CommitTransaction(CancellationToken cancellationToken = default);
            Task<int> SaveChanges(CancellationToken cancellationToken = default, bool clearChangeTracker = false);
            Task<int> SaveWithConcurrencyOverride(int retryCount = 1);
            /// <summary>
            /// Update an entity from the database
            /// </summary>
            /// <typeparam name="TEntity">The entity type</typeparam>
            /// <param name="entity">the item to be updated</param>
            /// <returns></returns>
            Task Reload<TEntity>(TEntity entity)
                where TEntity : class;
            void DisposeContext();

        
    }
}
