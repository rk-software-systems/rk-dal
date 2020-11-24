using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RKSoftware.DAL.Contract;
using RKSoftware.DAL.InMemory.RegistrationExtensions;
using System;

namespace RKSoftware.DAL.InMemory.Tests
{
    [TestClass]
    public class DITests
    {
        private IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            services.UseInMemory()
                .AddReadonlyStorage()
                .AddStorage();

            return services.BuildServiceProvider();
        }

        [TestMethod]
        public void TestCollectionStorageRegistration()
        {
            var serviceProvider = CreateServiceProvider();
            var collectionStorage = serviceProvider.GetService<CollectionStorage>();
            Assert.IsNotNull(collectionStorage);
        }

        [TestMethod]
        public void TestSingletonRegistration()
        {
            var serviceProvider = CreateServiceProvider();
            CollectionStorage parentStorageLink = serviceProvider.GetService<CollectionStorage>(); ;
            CollectionStorage childStorageLink;
            CollectionStorage standaloneLink;

            using (var scope = serviceProvider.CreateScope())
            {
                childStorageLink = scope.ServiceProvider.GetService<CollectionStorage>();
            }

            using (var scope = serviceProvider.CreateScope())
            {
                standaloneLink = scope.ServiceProvider.GetService<CollectionStorage>();
            }

            Assert.IsTrue(ReferenceEquals(parentStorageLink, childStorageLink));
            Assert.IsTrue(ReferenceEquals(parentStorageLink, standaloneLink));
            Assert.IsTrue(ReferenceEquals(childStorageLink, standaloneLink));
        }

        [TestMethod]
        public void TestReadonlyStorageRegistration()
        {
            var provider = CreateServiceProvider();
            var readonlyStorage = provider.GetService<IReadonlyStorage>();
            Assert.IsNotNull(readonlyStorage);
        }

        [TestMethod]
        public void TestStorageRegistration()
        {
            var provider = CreateServiceProvider();
            var storage = provider.GetService<IStorage>();
            Assert.IsNotNull(storage);
        }
    }
}
