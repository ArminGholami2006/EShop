using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Domain.Models;
using System.Reflection;

namespace Ordering.Infrastructure.Data;

public class ApplicationContext : DbContext, IApplicationContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
