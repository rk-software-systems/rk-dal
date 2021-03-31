using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RKSoftware.DAL.EntityFramework.RegistrationExtentions;
using RKSoftware.DAL.EntityFramework.Domain;
using Microsoft.EntityFrameworkCore;
using RKSoftware.DAL.Contract;
using RKSoftware.DAL.EntityFramework.Tests.DB;

namespace RKSoftware.DAL.EntityFramework.Tests
{
    [TestClass]
    public class DITests
    {
        private static IServiceProvider CreateServiceProvider()
        {
            var services = DBContextInitializer.RegisterDBContext(nameof(DITests));
            services.AddRKEFStorages();

            return services.BuildServiceProvider();
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

        [TestMethod]
        public void TestTransactionalStorageRegistration()
        {
            var provider = CreateServiceProvider();
            var storage = provider.GetService<ITransactionalStorage>();
            Assert.IsNotNull(storage);
        }
    }
}
