using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RKSoftware.DAL.Core;
using RKSoftware.DAL.EntityFramework.RegistrationExtensions;
using RKSoftware.DAL.EntityFramework.Tests.DB;

namespace RKSoftware.DAL.EntityFramework.Tests.Storage;

/// <summary>
/// Tests for <see cref="EntityFrameworkStorage.RemoveByQueryAsync{T}(IQueryable{T}, CancellationToken)"/>.
/// The EF InMemory provider does not support ExecuteDeleteAsync, so these tests cover
/// the guard paths that run before the delete is executed.
/// </summary>
[TestClass]
public class EntityFrameworkRemoveByQueryTest
{
    private static ServiceProvider GetProvider(string dbName)
    {
        var services = DBContextInitializer.RegisterDBContext(dbName);
        services.AddRKEFStorages();
        return services.BuildServiceProvider();
    }

    private static TestEntity CreateEntity(long key, string value = "some string")
    {
        return new TestEntity
        {
            TestStringProperty = value,
            TestDateProperty = DateTime.UtcNow,
            TestLongProperty = key
        };
    }

    /// <summary>
    /// Reads entities using a separate scope (and DbContext), so the result reflects what is persisted in storage.
    /// </summary>
    private static async Task<List<TestEntity>> ReadAllAsync(ServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        return await dbContext.Set<TestEntity>().AsNoTracking().OrderBy(x => x.TestLongProperty).ToListAsync();
    }

    private static async Task SeedAsync(ServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        dbContext.Set<TestEntity>().AddRange(CreateEntity(10), CreateEntity(11));
        await dbContext.SaveChangesAsync();
    }

    private static async Task AssertSeedIntactAsync(ServiceProvider provider)
    {
        var persisted = await ReadAllAsync(provider);
        Assert.HasCount(2, persisted);
        Assert.AreEqual(10, persisted[0].TestLongProperty);
        Assert.AreEqual(11, persisted[1].TestLongProperty);
    }

    [TestMethod]
    public async Task TestRemoveByQueryNull()
    {
        using var provider = GetProvider(nameof(TestRemoveByQueryNull));
        await SeedAsync(provider);
        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await storage.RemoveByQueryAsync((IQueryable<TestEntity>)null!);
        });

        await AssertSeedIntactAsync(provider);
    }

    [TestMethod]
    public async Task TestRemoveByQueryCanceledToken()
    {
        using var provider = GetProvider(nameof(TestRemoveByQueryCanceledToken));
        await SeedAsync(provider);
        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await storage.RemoveByQueryAsync(storage.Set<TestEntity>(), cts.Token);
        });

        await AssertSeedIntactAsync(provider);
    }

    [TestMethod]
    public async Task TestRemoveByQueryInTransactionThrows()
    {
        using var provider = GetProvider(nameof(TestRemoveByQueryInTransactionThrows));
        await SeedAsync(provider);
        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        await storage.BeginTransactionAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await storage.RemoveByQueryAsync(storage.Set<TestEntity>().Where(x => x.TestLongProperty == 10));
        });

        await storage.CommitTransactionAsync();

        await AssertSeedIntactAsync(provider);
    }

    [TestMethod]
    public async Task TestRemoveByQueryWithPendingChangesThrows()
    {
        using var provider = GetProvider(nameof(TestRemoveByQueryWithPendingChangesThrows));
        await SeedAsync(provider);
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        dbContext.Set<TestEntity>().Add(CreateEntity(12));

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await storage.RemoveByQueryAsync(storage.Set<TestEntity>().Where(x => x.TestLongProperty == 10));
        });

        await AssertSeedIntactAsync(provider);
    }

    [TestMethod]
    public async Task TestRemoveByQueryNonRelationalProviderThrows()
    {
        using var provider = GetProvider(nameof(TestRemoveByQueryNonRelationalProviderThrows));
        await SeedAsync(provider);
        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await storage.RemoveByQueryAsync(storage.Set<TestEntity>().Where(x => x.TestLongProperty == 10));
        });

        await AssertSeedIntactAsync(provider);
    }
}
