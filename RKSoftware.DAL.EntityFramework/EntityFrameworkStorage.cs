using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RKSoftware.DAL.Core;
using RKSoftware.DAL.EntityFramework.EFExtensions;

namespace RKSoftware.DAL.EntityFramework;

/// <summary>
/// <see cref="ITransactionalStorage"/> implementation using in Entity Framework
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="EntityFrameworkStorage"/> class.
/// </remarks>
public class EntityFrameworkStorage(DbContext context) : EntityFrameworkReadonlyStorage(context), ITransactionalStorage
{
#pragma warning disable CA1805 // Do not initialize unnecessarily
    private bool _activeTransaction = false;
#pragma warning restore CA1805 // Do not initialize unnecessarily
    private static readonly SemaphoreSlim _commitSemaphore = new(1, 1);

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
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var entry = DbContext.Set<T>().Add(entity);

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

                    await DbContext.SaveChangesAsync(cancellationToken);
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
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

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

                    await DbContext.SaveChangesAsync(cancellationToken);
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
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

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

                    await DbContext.SaveChangesAsync(cancellationToken);
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
    /// <see cref="ITransactionalStorage.CommitTransactionAsync"/>
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        await _commitSemaphore.WaitAsync();
        try
        {
            _activeTransaction = false;
            await DbContext.SaveChangesAsync();
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
            DbContext.ChangeTracker.Clear();
            _activeTransaction = false;
        }
        finally
        {
            _commitSemaphore.Release();
        }
    }

    private async Task<EntityEntry<T>> AttachEntity<T>(T entity) where T : class
    {
        try
        {
            return DbContext.Set<T>().Attach(entity);
        }
        catch (InvalidOperationException)
        {
            var keyValues = DbContext.FindPrimaryKeyValues(entity);
            if (keyValues == null)
            {
                return DbContext.Set<T>().Attach(entity);
            }

            var dbEntity = await DbContext.FindAsync<T>(keyValues.ToArray());
            if(dbEntity != null)
            {
                DbContext.Entry(dbEntity).State = EntityState.Detached;
            }           

            return DbContext.Set<T>().Attach(entity);
        }
    }
}
