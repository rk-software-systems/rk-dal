using RKSoftware.DAL.Core;

namespace RKSoftware.DAL.InMemory;

/// <summary>
/// In memory realization of <see cref="IReadonlyStorage"/>
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="InMemoryReadonlyStorage"/> class.
/// </remarks>
/// <param name="collectionStorage"></param>
#pragma warning disable CA1063 // Implement IDisposable Correctly
public class InMemoryReadonlyStorage(CollectionStorage collectionStorage) : IReadonlyStorage
#pragma warning restore CA1063 // Implement IDisposable Correctly
{
    /// <summary>
    /// In memory storage
    /// </summary>
    private readonly CollectionStorage _storage = collectionStorage;

    /// <summary>
    /// In memory storage
    /// </summary>
    public CollectionStorage Storage => _storage;

    /// <summary>
    /// <see cref="IDisposable"/> implementation
    /// </summary>
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
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
