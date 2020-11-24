using Microsoft.VisualStudio.TestTools.UnitTesting;
using RKSoftware.DAL.InMemory.Tests.Domain;
using System.Linq;

namespace RKSoftware.DAL.InMemory.Tests
{
    [TestClass]
    public class ReadonlyStorageTests
    {
        [TestMethod]
        public void TestInMemoryCollectionIsImmutable()
        {
            var storage = new CollectionStorage();
            var readonlyStorage = new InMemoryReadonlyStorage(storage);
            var collection = readonlyStorage.Set<TestEntity>();
            var list = collection.ToList();
            list.Add(new TestEntity());

            collection = readonlyStorage.Set<TestEntity>();
            Assert.AreEqual(0, collection.Count());
        }

        [TestMethod]
        public void TestIfCollectionReturnsData()
        {
            var storage = new CollectionStorage();
            var testEntity = new TestEntity
            {
                TestLongProperty = 1,
                TestStringProperty = "someString"
            };

            storage.GetCollection<TestEntity>().Add(testEntity);

            var readonlyStorage = new InMemoryReadonlyStorage(storage);
            var collection = readonlyStorage.Set<TestEntity>();

            Assert.AreEqual(1, collection.Count());
            var entity = collection.FirstOrDefault();
            Assert.IsNotNull(entity);
            Assert.AreEqual(testEntity.TestStringProperty, entity.TestStringProperty);
            Assert.AreEqual(testEntity.TestLongProperty, entity.TestLongProperty);
        }
    }
}
