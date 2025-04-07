using System.Collections;
using System.Collections.Concurrent;

namespace RKSoftware.DAL.InMemory;

/// <summary>
/// This class is used as an in-memory storage of collections
/// </summary>
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

    /// <summary>
    /// Get stored collection of particular types
    /// </summary>
    /// <typeparam name="T">Type of collection</typeparam>
    /// <returns>Stored collection</returns>
    public ICollection<T> GetCollection<T>() where T : class
    {
        var col = _entitySets.GetOrAdd(typeof(T).Name, new List<T>()) as ICollection<T>;
        return col!;
    }
}
