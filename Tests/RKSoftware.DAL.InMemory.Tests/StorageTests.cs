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
}
