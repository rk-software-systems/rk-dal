using Microsoft.VisualStudio.TestTools.UnitTesting;
using RKSoftware.DAL.InMemory.Tests.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RKSoftware.DAL.InMemory.Tests
{
    [TestClass]
    public class StorageTests
    {
        private Tuple<CollectionStorage, InMemoryStorage> CreateStorage()
        {
            var collectionStorage = new CollectionStorage();
            var storage = new InMemoryStorage(collectionStorage);

            return new Tuple<CollectionStorage, InMemoryStorage>(collectionStorage, storage);
        }

        [TestMethod]
        public void TestAddMethod()
        {
            var storages = CreateStorage();
            var entity = new TestEntity()
            {
                TestLongProperty = 1,
                TestStringProperty = "test string"
            };

            storages.Item2.Add(entity);

            Assert.AreEqual(1, storages.Item1.GetCollection<TestEntity>().Count);
        }

        [TestMethod]
        public async Task TestAsyncAddMethod()
        {
            var storages = CreateStorage();
            var entity = new TestEntity()
            {
                TestLongProperty = 1,
                TestStringProperty = "test string"
            };

            await storages.Item2.AddAsync(entity);

            Assert.AreEqual(1, storages.Item1.GetCollection<TestEntity>().Count);
        }

        [TestMethod]
        public void TestRemove()
        {
            var storages = CreateStorage();
            storages.Item1.GetCollection<TestEntity>().Add(new TestEntity()
            {
                TestLongProperty = 1
            });
            storages.Item1.GetCollection<TestEntity>().Add(new TestEntity()
            {
                TestLongProperty = 2
            });

            var entityToBeDeleted = storages.Item2.Set<TestEntity>()
                .FirstOrDefault(x => x.TestLongProperty == 2);
            Assert.IsNotNull(entityToBeDeleted);

            Assert.IsTrue(storages.Item2.Remove(entityToBeDeleted));
            Assert.AreEqual(1, storages.Item1.GetCollection<TestEntity>().Count);
            var remainigEntity = storages.Item2.Set<TestEntity>().FirstOrDefault();
            Assert.IsNotNull(remainigEntity);
            Assert.AreEqual(1, remainigEntity.TestLongProperty);
        }

        [TestMethod]
        public async Task TestRemoveAsync()
        {
            var storages = CreateStorage();
            storages.Item1.GetCollection<TestEntity>().Add(new TestEntity()
            {
                TestLongProperty = 1
            });
            storages.Item1.GetCollection<TestEntity>().Add(new TestEntity()
            {
                TestLongProperty = 2
            });

            var entityToBeDeleted = storages.Item2.Set<TestEntity>()
                .FirstOrDefault(x => x.TestLongProperty == 2);
            Assert.IsNotNull(entityToBeDeleted);

            Assert.IsTrue(await storages.Item2.RemoveAsync(entityToBeDeleted));
            Assert.AreEqual(1, storages.Item1.GetCollection<TestEntity>().Count);
            var remainigEntity = storages.Item2.Set<TestEntity>().FirstOrDefault();
            Assert.IsNotNull(remainigEntity);
            Assert.AreEqual(1, remainigEntity.TestLongProperty);
        }
    }
}
