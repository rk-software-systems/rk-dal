using System.Threading;
using System.Threading.Tasks;

namespace RKSoftware.DAL.Contract
{
    /// <summary>
    /// This service is used as a READ / WRITE Storage abstraction
    /// </summary>
    public interface IStorage : IReadonlyStorage
    {
        /// <summary>
        /// Add existing entity in storage
        /// </summary>
        /// <typeparam name="T">Type of the entity to be saved</typeparam>
        /// <param name="entity">Entity to be added</param>
        /// <returns>Entity after save operation</returns>
        T Add<T>(T entity) where T : class;

        /// <summary>
        /// Add existing entity in storage async
        /// </summary>
        /// <typeparam name="T">Type of the entity to be added</typeparam>
        /// <param name="entity">Entity to be saver</param>
        /// <returns>Entity after save operation</returns>
        Task<T> AddAsync<T>(T entity) where T : class;

        /// <summary>
        /// Add existing entity in storage async
        /// </summary>
        /// <typeparam name="T">Type of the entity to be saved</typeparam>
        /// <param name="entity">Entity to be added</param>
        /// <param name="cancellationToken">Operation cancellation token</param>
        /// <returns>Entity after save operation</returns>
        Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class;

        /// <summary>
        /// Save existing entity in storage
        /// </summary>
        /// <typeparam name="T">Type of the entity to be saved</typeparam>
        /// <param name="entity">Entity to be saved</param>
        /// <returns>Entity after save operation</returns>
        T Save<T>(T entity) where T : class;

        /// <summary>
        /// Save existing entity in storage async
        /// </summary>
        /// <typeparam name="T">Type of the entity to be saved</typeparam>
        /// <param name="entity">Entity to be saved</param>
        /// <returns>Entity after save operation</returns>
        Task<T> SaveAsync<T>(T entity) where T : class;

        /// <summary>
        /// Save existing entity in storage async
        /// </summary>
        /// <typeparam name="T">Type of the entity to be saved</typeparam>
        /// <param name="entity">Entity to be saved</param>
        /// <param name="cancellationToken">Operation cancellation token</param>
        /// <returns>Entity after save operation</returns>
        Task<T> SaveAsync<T>(T entity, CancellationToken cancellationToken) where T : class;

        /// <summary>
        /// Remove entity from storage
        /// </summary>
        /// <typeparam name="T">Type of the entity to be removed</typeparam>
        /// <param name="entity">Entity to be removed</param>
        bool Remove<T>(T entity) where T : class;

        /// <summary>
        /// Remove entity from storage async
        /// </summary>
        /// <typeparam name="T">Type of the entity to be removed</typeparam>
        /// <param name="entity">Entity to be removed</param>
        Task<bool> RemoveAsync<T>(T entity) where T : class;

        /// <summary>
        /// Remove entity from storage async
        /// </summary>
        /// <typeparam name="T">Type of the entity to be removed</typeparam>
        /// <param name="entity">Entity to be removed</param>
        /// <param name="cancellationToken">Operation cancellation token</param>
        Task<bool> RemoveAsync<T>(T entity, CancellationToken cancellationToken) where T : class;
    }
}
