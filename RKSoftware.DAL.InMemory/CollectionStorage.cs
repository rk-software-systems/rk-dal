using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RKSoftware.DAL.InMemory
{
    public class CollectionStorage
    {
        private readonly ConcurrentDictionary<string, ICollection> _entitySets;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionStorage"/> class.
        /// </summary>
        public CollectionStorage()
        {
            _entitySets = new ConcurrentDictionary<string, ICollection>();
        }

        public ICollection<T> GetCollection<T>() where T : class
        {
            return _entitySets.GetOrAdd(typeof(T).Name, new List<T>()) as ICollection<T>;
        }
    }
}
