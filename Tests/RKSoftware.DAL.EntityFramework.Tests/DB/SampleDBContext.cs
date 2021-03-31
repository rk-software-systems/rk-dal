using Microsoft.EntityFrameworkCore;

namespace RKSoftware.DAL.EntityFramework.Domain
{
    public class SampleDBContext: DbContext
    {
        public SampleDBContext(DbContextOptions<SampleDBContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TestEntityMap());
        }
    }
}
