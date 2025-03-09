using Microsoft.Extensions.DependencyInjection;
using RKSoftware.DAL.Core;

namespace RKSoftware.DAL.EntityFramework;

/// <summary>
/// Entity Framework implementation of <see cref="IQueryStorage"/>
/// </summary>
/// <param name="serviceProvider"><see cref="IServiceProvider"/> that is used to Resolve Storage Service</param>
public class EntityFrameworkQueryStorage(IServiceProvider serviceProvider) : IQueryStorage
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    /// <summary>
    /// <see cref="IQueryStorage.Query{TResult, TQueryable}(Func{IReadonlyStorage, IQueryable{TQueryable}}, Func{IQueryable{TQueryable}, TResult})"/>
    /// </summary>
    public TResult Query<TResult, TQueryable>(
        Func<IReadonlyStorage, IQueryable<TQueryable>> queryBuilder, 
        Func<IQueryable<TQueryable>, TResult> resultExecutor)
    {
        ArgumentNullException.ThrowIfNull(queryBuilder, nameof(queryBuilder));
        ArgumentNullException.ThrowIfNull(resultExecutor, nameof(resultExecutor));

        using var scope = _serviceProvider.CreateScope();
        using var storage = scope.ServiceProvider.GetRequiredService<IReadonlyStorage>();
        return resultExecutor(queryBuilder(storage));
    }

    /// <summary>
    /// <see cref="IQueryStorage.QueryAsync{TResult, TQueryable}(Func{IReadonlyStorage, Task{IQueryable{TQueryable}}}, Func{IQueryable{TQueryable}, TResult})"/>
    /// </summary>
    public async Task<TResult> QueryAsync<TResult, TQueryable>(
        Func<IReadonlyStorage, Task<IQueryable<TQueryable>>> queryBuilder, 
        Func<IQueryable<TQueryable>, TResult> resultExecutor)
    {
        ArgumentNullException.ThrowIfNull(queryBuilder, nameof(queryBuilder));
        ArgumentNullException.ThrowIfNull(resultExecutor, nameof(resultExecutor));

        using var scope = _serviceProvider.CreateScope();
        using var storage = scope.ServiceProvider.GetRequiredService<IReadonlyStorage>();
        return resultExecutor(await queryBuilder(storage));
    }

    /// <summary>
    /// <see cref="IQueryStorage.QueryAsync{TResult, TQueryable}(Func{IReadonlyStorage, Task{IQueryable{TQueryable}}}, Func{IQueryable{TQueryable}, Task{TResult}})"/>
    /// </summary>
    public async Task<TResult> QueryAsync<TResult, TQueryable>(
        Func<IReadonlyStorage, Task<IQueryable<TQueryable>>> queryBuilder, 
        Func<IQueryable<TQueryable>, Task<TResult>> resultExecutor)
    {
        ArgumentNullException.ThrowIfNull(queryBuilder, nameof(queryBuilder));
        ArgumentNullException.ThrowIfNull(resultExecutor, nameof(resultExecutor));

        using var scope = _serviceProvider.CreateScope();
        using var storage = scope.ServiceProvider.GetRequiredService<IReadonlyStorage>();
        return await resultExecutor(await queryBuilder(storage));
    }

    /// <summary>
    /// <see cref="IQueryStorage.QueryAsync{TResult, TQueryable}(Func{IReadonlyStorage, IQueryable{TQueryable}}, Func{IQueryable{TQueryable}, Task{TResult}})"/>
    /// </summary>
    public async Task<TResult> QueryAsync<TResult, TQueryable>(
        Func<IReadonlyStorage, IQueryable<TQueryable>> queryBuilder, 
        Func<IQueryable<TQueryable>, Task<TResult>> resultExecutor)
    {
        ArgumentNullException.ThrowIfNull(queryBuilder, nameof(queryBuilder));
        ArgumentNullException.ThrowIfNull(resultExecutor, nameof(resultExecutor));

        using var scope = _serviceProvider.CreateScope();
        using var storage = scope.ServiceProvider.GetRequiredService<IReadonlyStorage>();
        return await resultExecutor(queryBuilder(storage));
    }
}
