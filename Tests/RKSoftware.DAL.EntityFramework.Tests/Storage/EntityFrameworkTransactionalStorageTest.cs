using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RKSoftware.DAL.Core;
using RKSoftware.DAL.EntityFramework.RegistrationExtensions;
using RKSoftware.DAL.EntityFramework.Tests.DB;

namespace RKSoftware.DAL.EntityFramework.Tests.Storage;

[TestClass]
public class EntityFrameworkTransactionalStorageTest
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

    private static async Task SeedAsync(ServiceProvider provider, params TestEntity[] entities)
    {
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        dbContext.Set<TestEntity>().AddRange(entities);
        await dbContext.SaveChangesAsync();
    }

    [TestMethod]
    public async Task TestAddInTransactionNotPersistedUntilCommit()
    {
        using var provider = GetProvider(nameof(TestAddInTransactionNotPersistedUntilCommit));
        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();
        var entity = CreateEntity(10);

        await storage.BeginTransactionAsync();
        await storage.AddAsync(entity);

        Assert.IsEmpty(await ReadAllAsync(provider));

        await storage.CommitTransactionAsync();

        var persisted = await ReadAllAsync(provider);
        Assert.HasCount(1, persisted);
        Assert.IsTrue(persisted[0].CompareTo(entity));
    }

    [TestMethod]
    public async Task TestUpdateInTransactionNotPersistedUntilCommit()
    {
        using var provider = GetProvider(nameof(TestUpdateInTransactionNotPersistedUntilCommit));
        var original = CreateEntity(10);
        await SeedAsync(provider, original);

        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();
        var updated = CreateEntity(10, "some updated string");

        await storage.BeginTransactionAsync();
        await storage.SaveAsync(updated);

        var beforeCommit = await ReadAllAsync(provider);
        Assert.HasCount(1, beforeCommit);
        Assert.IsTrue(beforeCommit[0].CompareTo(original));

        await storage.CommitTransactionAsync();

        var afterCommit = await ReadAllAsync(provider);
        Assert.HasCount(1, afterCommit);
        Assert.IsTrue(afterCommit[0].CompareTo(updated));
    }

    [TestMethod]
    public async Task TestRemoveInTransactionNotPersistedUntilCommit()
    {
        using var provider = GetProvider(nameof(TestRemoveInTransactionNotPersistedUntilCommit));
        await SeedAsync(provider, CreateEntity(10));

        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        await storage.BeginTransactionAsync();
        var removed = await storage.RemoveAsync(new TestEntity { TestLongProperty = 10 });

        Assert.IsTrue(removed);
        Assert.HasCount(1, await ReadAllAsync(provider));

        await storage.CommitTransactionAsync();

        Assert.IsEmpty(await ReadAllAsync(provider));
    }

    [TestMethod]
    public async Task TestMultipleOperationsCommittedTogether()
    {
        using var provider = GetProvider(nameof(TestMultipleOperationsCommittedTogether));
        var original = CreateEntity(10);
        await SeedAsync(provider, original);

        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();
        var updated = CreateEntity(10, "some updated string");
        var added1 = CreateEntity(11);
        var added2 = CreateEntity(12);

        await storage.BeginTransactionAsync();
        await storage.AddAsync(added1);
        await storage.AddAsync(added2);
        await storage.SaveAsync(updated);

        var beforeCommit = await ReadAllAsync(provider);
        Assert.HasCount(1, beforeCommit);
        Assert.IsTrue(beforeCommit[0].CompareTo(original));

        await storage.CommitTransactionAsync();

        var afterCommit = await ReadAllAsync(provider);
        Assert.HasCount(3, afterCommit);
        Assert.IsTrue(afterCommit[0].CompareTo(updated));
        Assert.IsTrue(afterCommit[1].CompareTo(added1));
        Assert.IsTrue(afterCommit[2].CompareTo(added2));
    }

    [TestMethod]
    public async Task TestResetDiscardsChanges()
    {
        using var provider = GetProvider(nameof(TestResetDiscardsChanges));
        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        await storage.BeginTransactionAsync();
        await storage.AddAsync(CreateEntity(10));
        await storage.ResetTransactionAsync();

        Assert.IsEmpty(await ReadAllAsync(provider));

        await storage.CommitTransactionAsync();

        Assert.IsEmpty(await ReadAllAsync(provider));
    }

    [TestMethod]
    public async Task TestAutoSaveAfterCommit()
    {
        using var provider = GetProvider(nameof(TestAutoSaveAfterCommit));
        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        await storage.BeginTransactionAsync();
        await storage.AddAsync(CreateEntity(10));
        await storage.CommitTransactionAsync();

        await storage.AddAsync(CreateEntity(11));

        Assert.HasCount(2, await ReadAllAsync(provider));
    }

    [TestMethod]
    public async Task TestAutoSaveAfterReset()
    {
        using var provider = GetProvider(nameof(TestAutoSaveAfterReset));
        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        await storage.BeginTransactionAsync();
        await storage.AddAsync(CreateEntity(10));
        await storage.ResetTransactionAsync();

        var entity = CreateEntity(11);
        await storage.AddAsync(entity);

        var persisted = await ReadAllAsync(provider);
        Assert.HasCount(1, persisted);
        Assert.IsTrue(persisted[0].CompareTo(entity));
    }

    [TestMethod]
    public async Task TestCommitWithoutChanges()
    {
        using var provider = GetProvider(nameof(TestCommitWithoutChanges));
        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        await storage.BeginTransactionAsync();
        await storage.CommitTransactionAsync();

        Assert.IsEmpty(await ReadAllAsync(provider));
    }

    [TestMethod]
    public async Task TestCommitWithoutActiveTransactionDoesNothing()
    {
        using var provider = GetProvider(nameof(TestCommitWithoutActiveTransactionDoesNothing));
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        dbContext.Set<TestEntity>().Add(CreateEntity(10));

        await storage.CommitTransactionAsync();

        Assert.IsEmpty(await ReadAllAsync(provider));
        Assert.HasCount(1, dbContext.ChangeTracker.Entries());
    }

    [TestMethod]
    public async Task TestCommitClearsChangeTracker()
    {
        using var provider = GetProvider(nameof(TestCommitClearsChangeTracker));
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        await storage.BeginTransactionAsync();
        await storage.AddAsync(CreateEntity(10));

        Assert.IsNotEmpty(dbContext.ChangeTracker.Entries());

        await storage.CommitTransactionAsync();

        Assert.IsEmpty(dbContext.ChangeTracker.Entries());
    }

    [TestMethod]
    public async Task TestFailedCommitClearsStateAndEndsTransaction()
    {
        using var provider = GetProvider(nameof(TestFailedCommitClearsStateAndEndsTransaction));
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();

        await storage.BeginTransactionAsync();
        await storage.SaveAsync(CreateEntity(10));

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
        {
            await storage.CommitTransactionAsync();
        });

        Assert.IsEmpty(dbContext.ChangeTracker.Entries());

        await storage.AddAsync(CreateEntity(11));

        Assert.HasCount(1, await ReadAllAsync(provider));
    }

    [TestMethod]
    public async Task TestBeginTransactionWithCanceledToken()
    {
        using var provider = GetProvider(nameof(TestBeginTransactionWithCanceledToken));
        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await storage.BeginTransactionAsync(cts.Token);
        });

        await storage.AddAsync(CreateEntity(10));

        Assert.HasCount(1, await ReadAllAsync(provider));
    }

    [TestMethod]
    public async Task TestCommitTransactionWithCanceledToken()
    {
        using var provider = GetProvider(nameof(TestCommitTransactionWithCanceledToken));
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await storage.BeginTransactionAsync();
        await storage.AddAsync(CreateEntity(10));

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await storage.CommitTransactionAsync(cts.Token);
        });

        Assert.IsEmpty(await ReadAllAsync(provider));
        Assert.HasCount(1, dbContext.ChangeTracker.Entries());

        await storage.CommitTransactionAsync();

        Assert.HasCount(1, await ReadAllAsync(provider));
    }

    [TestMethod]
    public async Task TestResetTransactionWithCanceledToken()
    {
        using var provider = GetProvider(nameof(TestResetTransactionWithCanceledToken));
        using var scope = provider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<ITransactionalStorage>();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await storage.BeginTransactionAsync();
        await storage.AddAsync(CreateEntity(10));

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await storage.ResetTransactionAsync(cts.Token);
        });

        await storage.AddAsync(CreateEntity(11));

        Assert.IsEmpty(await ReadAllAsync(provider));

        await storage.CommitTransactionAsync();

        Assert.HasCount(2, await ReadAllAsync(provider));
    }
}
