namespace RKSoftware.DAL.Core;

/// <summary>
/// This service is used to query Readonly storage.
/// It is designed by this way to make it available to work thread safe
/// </summary>
public interface IQueryStorage
{
    /// <summary>
    /// Query Storage
    /// </summary>
    /// <typeparam name="TResult">Result of query execution</typeparam>
    /// <typeparam name="TQueryable">DB Query set result</typeparam>
    /// <param name="queryBuilder">Delegate that is used to build Storage query</param>
    /// <param name="resultExecutor">Delegate that is used to convert Storage query to result</param>
    /// <returns>Query result</returns>
    TResult Query<TResult, TQueryable>(
        Func<IReadonlyStorage, IQueryable<TQueryable>> queryBuilder,
        Func<IQueryable<TQueryable>, TResult> resultExecutor);

    /// <summary>
    /// Query Storage
    /// </summary>
    /// <typeparam name="TResult">Result of query execution</typeparam>
    /// <typeparam name="TQueryable">DB Query set result</typeparam>
    /// <param name="queryBuilder">Delegate that is used to build Storage query</param>
    /// <param name="resultExecutor">Delegate that is used to convert Storage query to result</param>
    /// <returns>Query result</returns>
    Task<TResult> QueryAsync<TResult, TQueryable>(
        Func<IReadonlyStorage, Task<IQueryable<TQueryable>>> queryBuilder,
        Func<IQueryable<TQueryable>, TResult> resultExecutor);

    /// <summary>
    /// Query Storage
    /// </summary>
    /// <typeparam name="TResult">Result of query execution</typeparam>
    /// <typeparam name="TQueryable">DB Query set result</typeparam>
    /// <param name="queryBuilder">Delegate that is used to build Storage query</param>
    /// <param name="resultExecutor">Delegate that is used to convert Storage query to result</param>
    /// <returns>Query result</returns>
    Task<TResult> QueryAsync<TResult, TQueryable>(
        Func<IReadonlyStorage, Task<IQueryable<TQueryable>>> queryBuilder,
        Func<IQueryable<TQueryable>, Task<TResult>> resultExecutor);

    /// <summary>
    /// Query Storage
    /// </summary>
    /// <typeparam name="TResult">Result of query execution</typeparam>
    /// <typeparam name="TQueryable">DB Query set result</typeparam>
    /// <param name="queryBuilder">Delegate that is used to build Storage query</param>
    /// <param name="resultExecutor">Delegate that is used to convert Storage query to result</param>
    /// <returns>Query result</returns>
    Task<TResult> QueryAsync<TResult, TQueryable>(
        Func<IReadonlyStorage, IQueryable<TQueryable>> queryBuilder,
        Func<IQueryable<TQueryable>, Task<TResult>> resultExecutor);
}
