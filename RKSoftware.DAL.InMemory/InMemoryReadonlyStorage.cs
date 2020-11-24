using RKSoftware.DAL.Contract;
using System.Linq;

namespace RKSoftware.DAL.InMemory
{
    public class InMemoryReadonlyStorage : IReadonlyStorage
    {
        protected readonly CollectionStorage _storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryReadonlyStorage"/> class.
        /// </summary>
        /// <param name="collectionStorage"></param>
        public InMemoryReadonlyStorage(CollectionStorage collectionStorage)
        {
            _storage = collectionStorage;
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return _storage.GetCollection<T>()
                .AsQueryable();
        }
    }
}
