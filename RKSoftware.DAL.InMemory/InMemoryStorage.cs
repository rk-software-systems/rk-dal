using RKSoftware.DAL.Contract;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RKSoftware.DAL.InMemory
{
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

        public T Add<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _storage.GetCollection<T>().Add(entity);

            return entity;
        }

        public async Task<T> AddAsync<T>(T entity) where T : class
        {
            return await Task.FromResult(Add(entity));
        }

        public async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            return await Task.FromResult(Add(entity));
        }

        public bool Remove<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return _storage.GetCollection<T>().Remove(entity);
        }

        public async Task<bool> RemoveAsync<T>(T entity) where T : class
        {
            return await Task.FromResult(Remove(entity));
        }

        public async Task<bool> RemoveAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            return await Task.FromResult(Remove(entity));
        }

        public T Save<T>(T entity) where T : class
        {
            return entity;
        }

        public Task<T> SaveAsync<T>(T entity) where T : class
        {
            return Task.FromResult(Save(entity));
        }

        public Task<T> SaveAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            return Task.FromResult(Save(entity));
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return _storage.GetCollection<T>()
                .AsQueryable();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
