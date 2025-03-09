using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RKSoftware.DAL.Core;
using RKSoftware.DAL.EntityFramework.RegistrationExtensions;
using RKSoftware.DAL.EntityFramework.Tests.DB;

namespace RKSoftware.DAL.EntityFramework.Tests.Storage;

[TestClass]
public class EntityFrameworkStorageTest
{
    private static IServiceScope GetScope(string dbName)
    {
        var services = DBContextInitializer.RegisterDBContext(dbName);
        services.AddRKEFStorages();
        return services.BuildServiceProvider().CreateScope();
    }

    [TestMethod]
    public async Task TestAdd()
    {
        using var scope = GetScope(nameof(TestAdd));
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<IStorage>();
        var entity = new TestEntity
        {
            TestStringProperty = "some string",
            TestDateProperty = DateTime.UtcNow,
            TestLongProperty = 10
        };
        await storage.AddAsync(entity);
        var entityFromContext = await dbContext.Set<TestEntity>().FirstOrDefaultAsync();
        Assert.IsNotNull(entityFromContext);
        Assert.IsTrue(entityFromContext.CompareTo(entity));
    }

    [TestMethod]
    public async Task TestDoubleAdd()
    {
        using var scope = GetScope(nameof(TestDoubleAdd));
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<IStorage>();
        var entity = new TestEntity
        {
            TestStringProperty = "some string",
            TestDateProperty = DateTime.UtcNow,
            TestLongProperty = 10
        };
        await storage.AddAsync(entity);
        await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
        {
            await storage.AddAsync(entity);
        });
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        using var scope = GetScope(nameof(TestUpdate));
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<IStorage>();

        var entity = new TestEntity
        {
            TestStringProperty = "some string",
            TestDateProperty = DateTime.UtcNow,
            TestLongProperty = 10
        };

        dbContext.Set<TestEntity>().Add(entity);
        await dbContext.SaveChangesAsync();

        entity.TestDateProperty = DateTime.UtcNow;

        var newContextEntity = new TestEntity()
        {
            TestLongProperty = entity.TestLongProperty,
            TestDateProperty = DateTime.UtcNow,
            TestStringProperty = "some updated string"
        };

        await storage.SaveAsync(newContextEntity);
        var entityFromContext = await dbContext.Set<TestEntity>().FirstOrDefaultAsync();
        Assert.IsNotNull(entityFromContext);
        Assert.IsTrue(entityFromContext.CompareTo(newContextEntity));
    }

    [TestMethod]
    public async Task TestUpdateNonExistent()
    {
        using var scope = GetScope(nameof(TestUpdateNonExistent));
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<IStorage>();

        var entity = new TestEntity
        {
            TestStringProperty = "some string",
            TestDateProperty = DateTime.UtcNow,
            TestLongProperty = 10
        };

        await Assert.ThrowsExceptionAsync<DbUpdateConcurrencyException>(async () =>
        {
            await storage.SaveAsync(entity);
        });
    }

    [TestMethod]
    public async Task TestRemove()
    {
        using var scope = GetScope(nameof(TestUpdate));
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<IStorage>();

        var entity = new TestEntity
        {
            TestStringProperty = "some string",
            TestDateProperty = DateTime.UtcNow,
            TestLongProperty = 10
        };

        dbContext.Set<TestEntity>().Add(entity);
        await dbContext.SaveChangesAsync();

        var entityToRemove = new TestEntity
        {
            TestLongProperty = entity.TestLongProperty
        };
        await storage.RemoveAsync(entityToRemove);

        var res = await dbContext.Set<TestEntity>().FindAsync(entity.TestLongProperty);

        Assert.IsNull(res);
    }

    [TestMethod]
    public async Task TestRemoveNonExisting()
    {
        using var scope = GetScope(nameof(TestUpdate));
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<IStorage>();

        var entityToRemove = new TestEntity
        {
            TestLongProperty = 10
        };
        await Assert.ThrowsExceptionAsync<DbUpdateConcurrencyException>(async () =>
        {
            await storage.RemoveAsync(entityToRemove);
        });
    }
}
