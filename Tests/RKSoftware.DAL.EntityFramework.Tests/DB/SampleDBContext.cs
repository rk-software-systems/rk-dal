using Microsoft.EntityFrameworkCore;

namespace RKSoftware.DAL.EntityFramework.Tests.DB;

public class SampleDBContext(DbContextOptions<SampleDBContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));

        modelBuilder.ApplyConfiguration(new TestEntityMap());
    }
}
