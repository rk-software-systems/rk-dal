using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RKSoftware.DAL.EntityFramework.Tests.DB;

public class TestEntityMap : IEntityTypeConfiguration<TestEntity>
{
    public void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));

        builder.HasKey(x => x.TestLongProperty);

        builder.Property(x => x.TestStringProperty)
            .IsRequired();
    }
}
