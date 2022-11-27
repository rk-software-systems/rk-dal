using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RKSoftware.DAL.Contract;
using RKSoftware.DAL.EntityFramework.EFExtensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//using System.Linq;

namespace RKSoftware.DAL.EntityFramework
{
    /// <summary>
    /// <see cref="ITransactionalStorage"/> implelmentation using in Entity Framework
    /// </summary>
    public class EntityFrameworkStorage : EntityFrameworkReadonlyStorage, ITransactionalStorage
    {
        private readonly DbContext _dbContext;
        private bool _activeTransaction;
        private static readonly SemaphoreSlim _commitSemaphore = new(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkStorage"/> class.
        /// </summary>
        public EntityFrameworkStorage(DbContext context)
            : base(context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _activeTransaction = false;
        }

        /// <summary>
        /// <see cref="IStorage.AddAsync{T}(T)"/>
        /// </summary>
        public async Task<T> AddAsync<T>(T entity) where T : class
        {
            return await AddAsync(entity, CancellationToken.None);
        }

        /// <summary>
        /// <see cref="IStorage.AddAsync{T}(T, CancellationToken)"/>
        /// </summary>
        public async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = _dbContext.Set<T>().Add(entity);

            if (!_activeTransaction)
            {
                await _commitSemaphore.WaitAsync(cancellationToken);
                try
                {
                    if (!_activeTransaction)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            entry.State = EntityState.Detached;
                            return entity;
                        }

                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }
                }
                finally
                {
                    _commitSemaphore.Release();
                }
            }

            return entry.Entity;
        }

        /// <summary>
        /// <see cref="ITransactionalStorage.BeginTransaction"/>
        /// </summary>
        public void BeginTransaction()
        {
            _activeTransaction = true;
        }

        /// <summary>
        /// <see cref="IStorage.RemoveAsync{T}(T)"/>
        /// </summary>
        public async Task<bool> RemoveAsync<T>(T entity) where T : class
        {
            return await RemoveAsync(entity, CancellationToken.None);
        }

        /// <summary>
        /// <see cref="IStorage.RemoveAsync{T}(T, CancellationToken)"/>
        /// </summary>
        public async Task<bool> RemoveAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = await AttachEntity(entity);
            entry.State = EntityState.Deleted;

            if (!_activeTransaction)
            {
                await _commitSemaphore.WaitAsync(cancellationToken);
                try
                {
                    if (!_activeTransaction)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            entry.State = EntityState.Detached;
                            return false;
                        }

                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }
                }
                finally
                {
                    _commitSemaphore.Release();
                }
            }

            return true;
        }

        /// <summary>
        /// <see cref="IStorage.SaveAsync{T}(T)"/>
        /// </summary>
        public async Task<T> SaveAsync<T>(T entity) where T : class
        {
            return await SaveAsync(entity, CancellationToken.None);
        }

        /// <summary>
        /// <see cref="IStorage.SaveAsync{T}(T, CancellationToken)"/>
        /// </summary>
        public async Task<T> SaveAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = await AttachEntity(entity);
            entry.State = EntityState.Modified;

            if (!_activeTransaction)
            {
                await _commitSemaphore.WaitAsync(cancellationToken);
                try
                {
                    if (!_activeTransaction)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            entry.State = EntityState.Detached;
                            return entity;
                        }

                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }
                }
                finally
                {
                    entry.State = EntityState.Detached;
                    _commitSemaphore.Release();
                }
            }

            return entry.Entity;
        }

        /// <summary>
        /// <see cref="ITransactionalStorage.CommitTrnsactionAsync"/>
        /// </summary>
        public async Task CommitTrnsactionAsync()
        {
            await _commitSemaphore.WaitAsync();
            try
            {
                _activeTransaction = false;
                await _dbContext.SaveChangesAsync();
            }
            finally
            {
                _commitSemaphore.Release();
            }
        }

        /// <summary>
        /// <see cref="ITransactionalStorage.ResetTransactionAsync"/>
        /// </summary>
        public async Task ResetTransactionAsync()
        {
            await _commitSemaphore.WaitAsync();
            try
            {
                _dbContext.ChangeTracker.Clear();
                _activeTransaction = false;
            }
            finally
            {
                _commitSemaphore.Release();
            }
        }

        private async Task<EntityEntry<T>> AttachEntity<T>(T entity)
            where T : class
        {
            try
            {
                return _dbContext.Set<T>().Attach(entity);
            }
            catch (InvalidOperationException)
            {
                var keyValues = _dbContext.FindPrimaryKeyValues(entity);
                if (keyValues == null)
                {
                    return _dbContext.Set<T>().Attach(entity);
                }

                var dbEntity = await _dbContext.FindAsync<T>(keyValues.ToArray());
                _dbContext.Entry(dbEntity).State = EntityState.Detached;

                return _dbContext.Set<T>().Attach(entity);
            }
        }
    }
}
