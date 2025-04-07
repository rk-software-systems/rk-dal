using Microsoft.EntityFrameworkCore;
using RKSoftware.DAL.Core;

namespace RKSoftware.DAL.EntityFramework;

/// <summary>
/// <see cref="IReadonlyStorage"/> Entity Framework implementation
/// </summary>
public class EntityFrameworkReadonlyStorage(DbContext context) : IReadonlyStorage
{
    private readonly DbContext _dbContext = context ?? throw new ArgumentNullException(nameof(context));
    private bool _disposed;

    /// <summary>
    /// DbContext instance
    /// </summary>
    protected DbContext DbContext => _dbContext;

    /// <summary>
    /// <see cref="IReadonlyStorage.Set{T}"/>
    /// </summary>
    public IQueryable<T> Set<T>() where T : class
    {
        return _dbContext.Set<T>().AsNoTracking();
    }


    /// <summary>
    /// <see cref="IDisposable.Dispose"/>
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _dbContext.Dispose();
        }

        _disposed = true;
    }

    
}
