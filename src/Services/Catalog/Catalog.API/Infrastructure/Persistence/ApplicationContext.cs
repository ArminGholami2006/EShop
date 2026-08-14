using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure.Persistence;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(x =>
        {
            x.ToTable("Products");

            x.Property(p => p.Id)
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            x.Property(p => p.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(150)")
                .IsRequired();

            x.Property(p => p.Category)
                .HasColumnName("Category")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            x.Property(p => p.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(300)")
                .IsRequired(false);

            x.Property(p => p.ImageFile)
                .HasColumnName("ImageFile")
                .HasColumnType("nvarchar(250)")
                .IsRequired();

            x.Property(p => p.Price)
                .HasColumnName("Price")
                .HasColumnType("decimal(18, 4)")
                .IsRequired();
        });
    }
}
