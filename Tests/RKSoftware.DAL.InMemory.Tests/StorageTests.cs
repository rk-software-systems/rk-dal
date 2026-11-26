using Microsoft.VisualStudio.TestTools.UnitTesting;
using RKSoftware.DAL.InMemory.Tests.Domain;

namespace RKSoftware.DAL.InMemory.Tests;

[TestClass]
public class StorageTests
{
    private static InMemoryStorage CreateStorage()
    {
        var collectionStorage = new CollectionStorage();
        return new InMemoryStorage(collectionStorage);
    }

    [TestMethod]
    public async Task TestAddMethod()
    {
        using var storage = CreateStorage();
        var entity = new TestEntity()
        {
            TestLongProperty = 1,
            TestStringProperty = "test string"
        };

        await storage.AddAsync(entity);

        Assert.AreEqual(1, storage.Storage.GetCollection<TestEntity>().Count);
    }

    [TestMethod]
    public async Task TestAsyncAddMethod()
    {
        using var storage = CreateStorage();
        var entity = new TestEntity()
        {
            TestLongProperty = 1,
            TestStringProperty = "test string"
        };

        await storage.AddAsync(entity);

        Assert.AreEqual(1, storage.Storage.GetCollection<TestEntity>().Count);
    }

    [TestMethod]
    public async Task TestRemove()
    {
        using var storage = CreateStorage();
        storage.Storage.GetCollection<TestEntity>().Add(new TestEntity()
        {
            TestLongProperty = 1
        });
        storage.Storage.GetCollection<TestEntity>().Add(new TestEntity()
        {
            TestLongProperty = 2
        });

        var entityToBeDeleted = storage.Set<TestEntity>()
            .FirstOrDefault(x => x.TestLongProperty == 2);
        Assert.IsNotNull(entityToBeDeleted);

        Assert.IsTrue(await storage.RemoveAsync(entityToBeDeleted));
        Assert.AreEqual(1, storage.Storage.GetCollection<TestEntity>().Count);
        var remainigEntity = storage.Set<TestEntity>().FirstOrDefault();
        Assert.IsNotNull(remainigEntity);
        Assert.AreEqual(1, remainigEntity.TestLongProperty);
    }

    [TestMethod]
    public async Task TestRemoveAsync()
    {
        using var storage = CreateStorage();
        storage.Storage.GetCollection<TestEntity>().Add(new TestEntity()
        {
            TestLongProperty = 1
        });
        storage.Storage.GetCollection<TestEntity>().Add(new TestEntity()
        {
            TestLongProperty = 2
        });

        var entityToBeDeleted = storage.Set<TestEntity>()
            .FirstOrDefault(x => x.TestLongProperty == 2);
        Assert.IsNotNull(entityToBeDeleted);

        Assert.IsTrue(await storage.RemoveAsync(entityToBeDeleted));
        Assert.AreEqual(1, storage.Storage.GetCollection<TestEntity>().Count);
        var remainigEntity = storage.Set<TestEntity>().FirstOrDefault();
        Assert.IsNotNull(remainigEntity);
        Assert.AreEqual(1, remainigEntity.TestLongProperty);
    }

    private static InMemoryStorage CreateSeededStorage(params long[] keys)
    {
        var storage = CreateStorage();
        foreach (var key in keys)
        {
            storage.Storage.GetCollection<TestEntity>().Add(new TestEntity()
            {
                TestLongProperty = key
            });
        }

        return storage;
    }

    [TestMethod]
    public async Task TestRemoveByQuery()
    {
        using var storage = CreateSeededStorage(1, 2, 3);

        var removed = await storage.RemoveAsync(storage.Set<TestEntity>().Where(x => x.TestLongProperty >= 2));

        Assert.IsTrue(removed);
        var remaining = storage.Storage.GetCollection<TestEntity>();
        Assert.AreEqual(1, remaining.Count);
        Assert.AreEqual(1, remaining.First().TestLongProperty);
    }

    [TestMethod]
    public async Task TestRemoveByQueryNoMatches()
    {
        using var storage = CreateSeededStorage(1, 2);

        var removed = await storage.RemoveAsync(storage.Set<TestEntity>().Where(x => x.TestLongProperty > 10));

        Assert.IsFalse(removed);
        Assert.AreEqual(2, storage.Storage.GetCollection<TestEntity>().Count);
    }

    [TestMethod]
    public async Task TestRemoveByQueryWithCanceledToken()
    {
        using var storage = CreateSeededStorage(1, 2);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await storage.RemoveAsync(storage.Set<TestEntity>(), cts.Token);
        });

        Assert.AreEqual(2, storage.Storage.GetCollection<TestEntity>().Count);
    }

    [TestMethod]
    public async Task TestRemoveByQueryNull()
    {
        using var storage = CreateStorage();

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await storage.RemoveAsync((IQueryable<TestEntity>)null!);
        });
    }
}
