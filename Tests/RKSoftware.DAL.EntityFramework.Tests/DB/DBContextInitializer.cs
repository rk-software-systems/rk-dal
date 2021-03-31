using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RKSoftware.DAL.EntityFramework.Domain;

namespace RKSoftware.DAL.EntityFramework.Tests.DB
{
    public static class DBContextInitializer
    {
        public static IServiceCollection RegisterDBContext()
        {
            var services = new ServiceCollection();

            services.AddDbContextPool<SampleDBContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "SampleDB");
            });
            services.AddScoped<DbContext, SampleDBContext>();

            return services;
        }
    }
}
