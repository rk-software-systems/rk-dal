using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RKSoftware.DAL.EntityFramework.Domain
{
    public class TestEntityMap : IEntityTypeConfiguration<TestEntity>
    {
        public void Configure(EntityTypeBuilder<TestEntity> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasKey(x => x.TestLongProperty);

            builder.Property(x => x.TestStringProperty)
                .IsRequired();
        }
    }
}
