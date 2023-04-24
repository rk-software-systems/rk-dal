using RKSoftware.DAL.Contract;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RKSoftware.DAL.InMemory
{
    /// <summary>
    /// <see cref="IStorage"/> in memory implementation
    /// </summary>
    public class InMemoryStorage : IStorage
    {
        private readonly CollectionStorage _storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryStorage"/> class.
        /// </summary>
        public InMemoryStorage(CollectionStorage collectionStorage)
        {
            _storage = collectionStorage;
        }

        private T Add<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _storage.GetCollection<T>().Add(entity);

            return entity;
        }

        /// <summary>
        /// <see cref="IStorage.AddAsync{T}(T)"/>
        /// </summary>
        public async Task<T> AddAsync<T>(T entity) where T : class
        {
            return await Task.FromResult(Add(entity));
        }

        /// <summary>
        /// <see cref="IStorage.AddAsync{T}(T, CancellationToken)"/>
        /// </summary>
        public async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            return await Task.FromResult(Add(entity));
        }

        private bool Remove<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return _storage.GetCollection<T>().Remove(entity);
        }

        /// <summary>
        /// <see cref="IStorage.RemoveAsync{T}(T)"/>
        /// </summary>
        public async Task<bool> RemoveAsync<T>(T entity) where T : class
        {
            return await Task.FromResult(Remove(entity));
        }

        /// <summary>
        /// <see cref="IStorage.RemoveAsync{T}(T, CancellationToken)"/>
        /// </summary>
        public async Task<bool> RemoveAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            return await Task.FromResult(Remove(entity));
        }

        private T Save<T>(T entity) where T : class
        {
            return entity;
        }

        /// <summary>
        /// <see cref="IStorage.SaveAsync{T}(T)"/>
        /// </summary>
        public Task<T> SaveAsync<T>(T entity) where T : class
        {
            return Task.FromResult(Save(entity));
        }

        /// <summary>
        /// <see cref="IStorage.SaveAsync{T}(T, CancellationToken)"/>
        /// </summary>
        public Task<T> SaveAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            return Task.FromResult(Save(entity));
        }

        /// <summary>
        /// <see cref="IReadonlyStorage.Set{T}"/>
        /// </summary>
        public IQueryable<T> Set<T>() where T : class
        {
            return _storage.GetCollection<T>()
                .AsQueryable();
        }

        /// <summary>
        /// <see cref="IDisposable"/>
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
