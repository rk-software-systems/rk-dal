using Microsoft.EntityFrameworkCore;
using RKSoftware.DAL.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RKSoftware.DAL.EntityFramework
{
    public class EntityFrameworkStorage : EntityFrameworkReadonlyStorage, ITransactionalStorage
    {
        private readonly DbContext _dbContext;
        private bool _activeTransaction;
        private static SemaphoreSlim _commitSemaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkStorage"/> class.
        /// </summary>
        public EntityFrameworkStorage(DbContext context)
            : base(context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _dbContext = context;
            _activeTransaction = false;
        }


        public T Add<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = _dbContext.Set<T>().Add(entity);

            if (!_activeTransaction)
            {
                _commitSemaphore.Wait();
                try
                {
                    if (!_activeTransaction)
                    {
                        _dbContext.SaveChanges();
                    }
                }
                finally
                {
                    _commitSemaphore.Release();
                }
            }

            return entry.Entity;
        }

        public async Task<T> AddAsync<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = _dbContext.Set<T>().Add(entity);

            if (!_activeTransaction)
            {
                _commitSemaphore.Wait();
                try
                {
                    if (!_activeTransaction)
                    {
                        await _dbContext.SaveChangesAsync();
                    }
                }
                finally
                {
                    _commitSemaphore.Release();
                }
            }

            return entry.Entity;
        }

        public async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = _dbContext.Set<T>().Add(entity);

            if (!_activeTransaction)
            {
                _commitSemaphore.Wait();
                try
                {
                    if (!_activeTransaction)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            entry.State = EntityState.Detached;
                            return entity;
                        }

                        await _dbContext.SaveChangesAsync();
                    }
                }
                finally
                {
                    _commitSemaphore.Release();
                }
            }

            return entry.Entity;
        }

        public void BeginTransaction()
        {
            _activeTransaction = true;
        }

        public void CommitTrnsaction()
        {
            _commitSemaphore.Wait();
            try
            {
                _activeTransaction = false;
                _dbContext.SaveChanges();
            }
            finally
            {
                _commitSemaphore.Release();
            }
        }

        public bool Remove<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Remove(entity);
            if (!_activeTransaction)
            {
                _commitSemaphore.Wait();
                try
                {
                    if (!_activeTransaction)
                    {
                        _dbContext.SaveChanges();
                    }
                }
                finally
                {
                    _commitSemaphore.Release();
                }

            }

            return true;
        }

        public Task<bool> RemoveAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            throw new NotImplementedException();
        }

        public T Save<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> SaveAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> SaveAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
