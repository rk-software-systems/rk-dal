using System;
using Microsoft.EntityFrameworkCore;

namespace RKSoftware.DAL.EntityFramework.Domain
{
    public class SampleDBContext: DbContext
    {
        public SampleDBContext(DbContextOptions<SampleDBContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.ApplyConfiguration(new TestEntityMap());
        }
    }
}
