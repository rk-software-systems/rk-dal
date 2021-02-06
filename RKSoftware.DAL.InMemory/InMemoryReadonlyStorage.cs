using RKSoftware.DAL.Contract;
using System;
using System.Linq;

namespace RKSoftware.DAL.InMemory
{
    /// <summary>
    /// In memory realization of <see cref="IReadonlyStorage"/>
    /// </summary>
    public class InMemoryReadonlyStorage : IReadonlyStorage
    {
        /// <summary>
        /// In memory storage
        /// </summary>
        protected readonly CollectionStorage _storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryReadonlyStorage"/> class.
        /// </summary>
        /// <param name="collectionStorage"></param>
        public InMemoryReadonlyStorage(CollectionStorage collectionStorage)
        {
            _storage = collectionStorage;
        }

        /// <summary>
        /// <see cref="IDisposable"/> implelmentation
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <see cref="IReadonlyStorage.Set{T}"/>
        /// </summary>
        public IQueryable<T> Set<T>() where T : class
        {
            return _storage.GetCollection<T>()
                .AsQueryable();
        }
    }
}
