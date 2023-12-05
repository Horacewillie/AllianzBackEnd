using AllianzBackEnd.Domain;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T>
    where T : DbEntityBase
    {
        protected DatabaseContext DbContext;
        protected IDbContextTransaction _currentTransaction = null;

        public BaseRepository(DatabaseContext context)
        {
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
        }
        public T Add(T entity)
        {
            var savedEntity = DbContext.Set<T>().Add(entity).Entity;
            return savedEntity;
        }

        //public async Task<(List<TEntity>, PagingData)> GetPaginatedResults<TEntity>(
        //Func<IQueryable<TEntity>, IQueryable<TEntity>> criteria,
        //Expression<Func<TEntity, DateTime>> orderBy,
        //int pageNumber,
        //int pageSize,
        //bool orderByDescending) where TEntity : class
        //{
        //    IQueryable<TEntity> query = DbContext.Set<TEntity>();
        //    query = criteria(query);

        //    var (dbSet, pagingData) = query.Paginate(orderBy, pageNumber, pageSize, orderByDescending);
        //    return (await dbSet.ToListAsync(), pagingData);
        //}

        public async Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            _currentTransaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async virtual Task CommitTransaction(CancellationToken cancellationToken = default)
        {
            await _currentTransaction?.CommitAsync(cancellationToken)!;
            _currentTransaction?.Dispose();
        }

        public void DisposeContext()
        {
            ClearChangeTracker();
            DbContext.Dispose();
        }

        public async virtual Task<T> Find(params object[] keys) =>
            await DbContext.FindAsync<T>(keys);

        public Task Reload<TEntity>(TEntity entity)
            where TEntity : class
        {
            return DbContext.Entry(entity).ReloadAsync();
        }

        public async Task Rollback(CancellationToken cancellationToken = default)
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }

        public async Task<int> SaveChanges(CancellationToken cancellationToken = default,
         bool clearChangeTracker = false)
        {
            var result = await DbContext.SaveChangesAsync(cancellationToken);
            if (clearChangeTracker)
                ClearChangeTracker();
            return result;
        }

        private void ClearChangeTracker()
        {
            foreach (var entry in DbContext.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }

        public async Task<int> SaveWithConcurrencyOverride(int retryCount = 1)
        {
            for (int i = 0; i <= retryCount; i++)
            {
                try
                {
                    return await DbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException dbEx)
                {
                    foreach (var entry in dbEx.Entries)
                    {
                        var databaseValues = entry.GetDatabaseValues();
                        entry.OriginalValues.SetValues(databaseValues);
                        var rowVersion = entry.Property("RowVersion");
                        if (rowVersion != null) rowVersion.CurrentValue = databaseValues["RowVersion"];
                        //entry.CurrentValues["RowVersion"] = databaseValues["RowVersion"];                        
                        entry.State = EntityState.Modified;
                    }
                }
            }
            return 0;
        }

        public void Update(T entity)
        {
            DbContext.Set<T>().Update(entity);
        }


    }
}
