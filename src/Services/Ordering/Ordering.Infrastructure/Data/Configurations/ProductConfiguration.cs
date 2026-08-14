using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .HasColumnType("uniqueidentifier")
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(x => x.Id)
            .HasConversion(productId => productId.Value, dbId => ProductId.Of(dbId));

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
