using System;
using System.Linq;
using System.Threading.Tasks;

namespace RKSoftware.DAL.Contract
{
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
        /// <typeparam name="TQueriable">DB Query set result</typeparam>
        /// <param name="queryBuilder">Delegate that is used to build Storage query</param>
        /// <param name="resultExecutor">Delegate that is used to convert Storage query to result</param>
        /// <returns>Query result</returns>
        TResult Query<TResult, TQueriable>(Func<IReadonlyStorage, IQueryable<TQueriable>> queryBuilder,
            Func<IQueryable<TQueriable>, TResult> resultExecutor);

        /// <summary>
        /// Query Storage
        /// </summary>
        /// <typeparam name="TResult">Result of query execution</typeparam>
        /// <typeparam name="TQueriable">DB Query set result</typeparam>
        /// <param name="queryBuilder">Delegate that is used to build Storage query</param>
        /// <param name="resultExecutor">Delegate that is used to convert Storage query to result</param>
        /// <returns>Query result</returns>
        Task<TResult> QueryAsync<TResult, TQueriable>(Func<IReadonlyStorage, Task<IQueryable<TQueriable>>> queryBuilder,
            Func<IQueryable<TQueriable>, TResult> resultExecutor);

        /// <summary>
        /// Query Storage
        /// </summary>
        /// <typeparam name="TResult">Result of query execution</typeparam>
        /// <typeparam name="TQueriable">DB Query set result</typeparam>
        /// <param name="queryBuilder">Delegate that is used to build Storage query</param>
        /// <param name="resultExecutor">Delegate that is used to convert Storage query to result</param>
        /// <returns>Query result</returns>
        Task<TResult> QueryAsync<TResult, TQueriable>(Func<IReadonlyStorage, Task<IQueryable<TQueriable>>> queryBuilder,
            Func<IQueryable<TQueriable>, Task<TResult>> resultExecutor);

        /// <summary>
        /// Query Storage
        /// </summary>
        /// <typeparam name="TResult">Result of query execution</typeparam>
        /// <typeparam name="TQueriable">DB Query set result</typeparam>
        /// <param name="queryBuilder">Delegate that is used to build Storage query</param>
        /// <param name="resultExecutor">Delegate that is used to convert Storage query to result</param>
        /// <returns>Query result</returns>
        Task<TResult> QueryAsync<TResult, TQueriable>(Func<IReadonlyStorage, IQueryable<TQueriable>> queryBuilder,
            Func<IQueryable<TQueriable>, Task<TResult>> resultExecutor);
    }
}
