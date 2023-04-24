using Microsoft.Extensions.DependencyInjection;
using RKSoftware.DAL.Contract;

namespace RKSoftware.DAL.InMemory.RegistrationExtensions
{
    /// <summary>
    /// This class contains In Memory storage service collection registration extension methods
    /// </summary>
    public static class InMemoryRegistration
    {
        /// <summary>
        /// Register In Memory storage 
        /// </summary>
        /// <param name="services">Service collection where to register storage</param>
        /// <returns></returns>
        public static IServiceCollection UseInMemory(this IServiceCollection services)
        {
            return services.AddSingleton<CollectionStorage>();
        }

        /// <summary>
        /// Register <see cref="IReadonlyStorage"/> in memory realization
        /// </summary>
        /// <param name="services">Service collection where to register storage</param>
        /// <returns></returns>
        public static IServiceCollection AddReadonlyStorage(this IServiceCollection services)
        {
            return services.AddScoped<IReadonlyStorage, InMemoryReadonlyStorage>();
        }

        /// <summary>
        /// Register <see cref="IStorage"/> in memory realization
        /// </summary>
        /// <param name="services">Service collection where to register storage</param>
        /// <returns></returns>
        public static IServiceCollection AddStorage(this IServiceCollection services)
        {
            return services.AddScoped<IStorage, InMemoryStorage>();
        }
    }
}
