using Microsoft.Extensions.DependencyInjection;
using RKSoftware.DAL.Contract;

namespace RKSoftware.DAL.InMemory.RegistrationExtensions
{
    public static class InMemoryRegistration
    {
        public static IServiceCollection UseInMemory(this IServiceCollection services)
        {
            return services.AddSingleton<CollectionStorage>();
        }

        public static IServiceCollection AddReadonlyStorage(this IServiceCollection services)
        {
            return services.AddScoped<IReadonlyStorage, InMemoryReadonlyStorage>();
        }

        public static IServiceCollection AddStorage(this IServiceCollection services)
        {
            return services.AddScoped<IStorage, InMemoryStorage>();
        }
    }
}
