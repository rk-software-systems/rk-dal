using RKSoftware.DAL.Core;

namespace RKSoftware.DAL.InMemory;

/// <summary>
/// <see cref="IStorage"/> in memory implementation
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="InMemoryStorage"/> class.
/// </remarks>
public class InMemoryStorage(CollectionStorage collectionStorage) : InMemoryReadonlyStorage(collectionStorage), IStorage
{
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

    /// <summary>
    /// <see cref="IStorage.RemoveAsync{T}(IQueryable{T})"/>
    /// </summary>
    public Task<bool> RemoveAsync<T>(IQueryable<T> query) where T : class
    {
        return Task.FromResult(RemoveRange(query));
    }

    /// <summary>
    /// <see cref="IStorage.RemoveAsync{T}(IQueryable{T}, CancellationToken)"/>
    /// </summary>
    public Task<bool> RemoveAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) where T : class
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(RemoveRange(query));
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
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public IQueryable<T> Set<T>() where T : class
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    {
        return Storage.GetCollection<T>()
            .AsQueryable();
    }

    private T Add<T>(T entity) where T : class
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        Storage.GetCollection<T>().Add(entity);

        return entity;
    }

    private static T Save<T>(T entity) where T : class
    {
        return entity;
    }

    private bool Remove<T>(T entity) where T : class
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        return Storage.GetCollection<T>().Remove(entity);
    }

    private bool RemoveRange<T>(IQueryable<T> query) where T : class
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        // Materialize first: the query may enumerate the same collection we remove from.
        var toRemove = query.ToList();
        var collection = Storage.GetCollection<T>();

        var removed = false;
        foreach (var item in toRemove)
        {
            removed |= collection.Remove(item);
        }

        return removed;
    }
}
