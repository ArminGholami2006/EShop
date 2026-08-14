using Basket.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Infrastructure.Persistence;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<ShoppingCart> ShoppingCarts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoppingCart>(x =>
        {
            x.ToTable("ShoppingCarts");

            x.HasKey(x => x.Username);

            x.Property(x => x.Username)
                .HasColumnName("Username")
                .HasColumnType("nvarchar(200)")
                .IsRequired();

            x.OwnsMany(x => x.Items);
        });
    }
}
