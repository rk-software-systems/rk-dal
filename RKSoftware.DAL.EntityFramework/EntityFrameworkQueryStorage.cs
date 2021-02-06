using Microsoft.Extensions.DependencyInjection;
using RKSoftware.DAL.Contract;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RKSoftware.DAL.EntityFramework
{
    /// <summary>
    /// Entity Framework implelmentation of <see cref="IQueryStorage"/>
    /// </summary>
    public class EntityFrameworkQueryStorage : IQueryStorage
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="IQueryStorage"/>
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> that is used to Resolve Storage Service</param>
        public EntityFrameworkQueryStorage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// <see cref="IQueryStorage.QueryAsync{TResult, TQueriable}(Func{IReadonlyStorage, IQueryable{TQueriable}}, Func{IQueryable{TQueriable}, TResult})"/>
        /// </summary>
        public TResult QueryAsync<TResult, TQueriable>(Func<IReadonlyStorage, IQueryable<TQueriable>> queryBuilder, Func<IQueryable<TQueriable>, TResult> resultExecutor)
        {
            using var scope = _serviceProvider.CreateScope();
            using var storage = _serviceProvider.GetRequiredService<IReadonlyStorage>();
            return resultExecutor(queryBuilder(storage));
        }

        /// <summary>
        /// <see cref="IQueryStorage.QueryAsync{TResult, TQueriable}(Func{IReadonlyStorage, Task{IQueryable{TQueriable}}}, Func{IQueryable{TQueriable}, TResult})"/>
        /// </summary>
        public async Task<TResult> QueryAsync<TResult, TQueriable>(Func<IReadonlyStorage, Task<IQueryable<TQueriable>>> queryBuilder, Func<IQueryable<TQueriable>, TResult> resultExecutor)
        {
            using var scope = _serviceProvider.CreateScope();
            using var storage = _serviceProvider.GetRequiredService<IReadonlyStorage>();
            return resultExecutor(await queryBuilder(storage));
        }

        /// <summary>
        /// <see cref="IQueryStorage.QueryAsync{TResult, TQueriable}(Func{IReadonlyStorage, Task{IQueryable{TQueriable}}}, Func{IQueryable{TQueriable}, Task{TResult}})"/>
        /// </summary>
        public async Task<TResult> QueryAsync<TResult, TQueriable>(Func<IReadonlyStorage, Task<IQueryable<TQueriable>>> queryBuilder, Func<IQueryable<TQueriable>, Task<TResult>> resultExecutor)
        {
            using var scope = _serviceProvider.CreateScope();
            using var storage = _serviceProvider.GetRequiredService<IReadonlyStorage>();
            return await resultExecutor(await queryBuilder(storage));
        }

        /// <summary>
        /// <see cref="IQueryStorage.QueryAsync{TResult, TQueriable}(Func{IReadonlyStorage, IQueryable{TQueriable}}, Func{IQueryable{TQueriable}, Task{TResult}})"/>
        /// </summary>
        public async Task<TResult> QueryAsync<TResult, TQueriable>(Func<IReadonlyStorage, IQueryable<TQueriable>> queryBuilder, Func<IQueryable<TQueriable>, Task<TResult>> resultExecutor)
        {
            using var scope = _serviceProvider.CreateScope();
            using var storage = _serviceProvider.GetRequiredService<IReadonlyStorage>();
            return await resultExecutor(queryBuilder(storage));
        }
    }
}
