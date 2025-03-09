using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RKSoftware.DAL.EntityFramework.RegistrationExtensions;
using RKSoftware.DAL.Core;
using RKSoftware.DAL.EntityFramework.Tests.DB;

namespace RKSoftware.DAL.EntityFramework.Tests;

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
