using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RKSoftware.DAL.EntityFramework.Tests.DB;

public static class DBContextInitializer
{
    public static IServiceCollection RegisterDBContext(string dbName)
    {
        var services = new ServiceCollection();

        services.AddDbContextPool<SampleDBContext>(options =>
        {
            options.UseInMemoryDatabase(databaseName: dbName);
        });
        services.AddScoped<DbContext, SampleDBContext>();

        return services;
    }
}
