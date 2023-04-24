using Microsoft.Extensions.DependencyInjection;
using RKSoftware.DAL.Contract;

namespace RKSoftware.DAL.EntityFramework.RegistrationExtentions
{
    /// <summary>
    /// This class contains Extension methods that can be used to Register RK Software Entity Framework DAL services
    /// </summary>
    public static class DependencyRegistration
    {
        /// <summary>
        /// Add <see cref="IReadonlyStorage"/> Entity Framework RK Storage implementations
        /// </summary>
        /// <param name="serviceCollection">Service collection to register storages</param>
        /// <returns></returns>
        public static IServiceCollection AddRKEFReadonlyStorage(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IReadonlyStorage, EntityFrameworkReadonlyStorage>();

            return serviceCollection;
        }

        /// <summary>
        /// Add <see cref="IQueryStorage"/> Entity Framework RK Storage implementations
        /// </summary>
        /// <param name="serviceCollection">Service collection to register storages</param>
        /// <returns></returns>
        public static IServiceCollection AddRKEFQueryStorage(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IQueryStorage, EntityFrameworkQueryStorage>();
            return serviceCollection;
        }

        /// <summary>
        /// Add <see cref="IStorage"/>, <see cref="ITransactionalStorage"/> Entity Framework RK Storage implementations
        /// </summary>
        /// <param name="serviceCollection">Service collection to register storages</param>
        /// <returns></returns>
        public static IServiceCollection AddRKEFStorage(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorage, EntityFrameworkStorage>();
            serviceCollection.AddScoped<ITransactionalStorage, EntityFrameworkStorage>();

            return serviceCollection;
        }

        /// <summary>
        /// Add all types of Entity Framework RK Storage implementations
        /// </summary>
        /// <param name="serviceCollection">Service collection to register storages</param>
        /// <returns></returns>
        public static IServiceCollection AddRKEFStorages(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddRKEFQueryStorage();
            serviceCollection.AddRKEFReadonlyStorage();
            serviceCollection.AddRKEFStorage();

            return serviceCollection;
        }
    }
}
