using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RKSoftware.DAL.Contract;
using RKSoftware.DAL.EntityFramework.Domain;
using RKSoftware.DAL.EntityFramework.RegistrationExtentions;
using RKSoftware.DAL.EntityFramework.Tests.DB;
using System;
using System.Threading.Tasks;

namespace RKSoftware.DAL.EntityFramework.Tests.Storage
{
    [TestClass]
    public class EnttiyFrameworkStorageTest
    {
        private static IServiceScope GetScope()
        {
            var services = DBContextInitializer.RegisterDBContext();
            services.AddRKEFStorages();
            return services.BuildServiceProvider().CreateScope();
        }

        [TestMethod]
        public async Task TestAdd()
        {
            using var scope = GetScope();
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
            using var scope = GetScope();
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
            using var scope = GetScope();
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
        public async Task TestDoubleUpdate()
        {
            using var scope = GetScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
            var storage = scope.ServiceProvider.GetRequiredService<IStorage>();

            var entity = new TestEntity
            {
                TestStringProperty = "some string",
                TestDateProperty = DateTime.UtcNow,
                TestLongProperty = 10
            };

            entity.TestDateProperty = DateTime.UtcNow;

            var newContextEntity = new TestEntity()
            {
                TestLongProperty = entity.TestLongProperty,
                TestDateProperty = DateTime.UtcNow,
                TestStringProperty = "some updated string"
            };

            await Assert.ThrowsExceptionAsync<DbUpdateConcurrencyException>(async () =>
            {
                await storage.SaveAsync(newContextEntity);
            });
        }
    }
}
