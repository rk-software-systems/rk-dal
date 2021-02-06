using Microsoft.Extensions.DependencyInjection;
using RKSoftware.DAL.Contract;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RKSoftware.DAL.EntityFramework
{
    public class EntityFrameworkQueryStorage : IQueryStorage
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkQueryStorage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TResult QueryAsync<TResult, TQueriable>(Func<IReadonlyStorage, IQueryable<TQueriable>> queryBuilder, Func<IQueryable<TQueriable>, TResult> resultExecutor)
        {
            using var storage = _serviceProvider.GetRequiredService<IReadonlyStorage>();
            return resultExecutor(queryBuilder(storage));
        }

        public async Task<TResult> QueryAsync<TResult, TQueriable>(Func<IReadonlyStorage, Task<IQueryable<TQueriable>>> queryBuilder, Func<IQueryable<TQueriable>, TResult> resultExecutor)
        {
            using var storage = _serviceProvider.GetRequiredService<IReadonlyStorage>();
            return resultExecutor(await queryBuilder(storage));
        }

        public async Task<TResult> QueryAsync<TResult, TQueriable>(Func<IReadonlyStorage, Task<IQueryable<TQueriable>>> queryBuilder, Func<IQueryable<TQueriable>, Task<TResult>> resultExecutor)
        {
            using var storage = _serviceProvider.GetRequiredService<IReadonlyStorage>();
            return await resultExecutor(await queryBuilder(storage));
        }

        public async Task<TResult> QueryAsync<TResult, TQueriable>(Func<IReadonlyStorage, IQueryable<TQueriable>> queryBuilder, Func<IQueryable<TQueriable>, Task<TResult>> resultExecutor)
        {

            using var storage = _serviceProvider.GetRequiredService<IReadonlyStorage>();
            return await resultExecutor(queryBuilder(storage));
        }
    }
}
