using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtension
{
    public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

        await context.Database.MigrateAsync();
    }

    public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

        await SeedCustomerAsync(context);
        await SeedProductAsync(context);
        await SeedOrderAndOrderItemsAsync(context);
    }

    private static async Task SeedCustomerAsync(ApplicationContext context)
    {
        if (!await context.Customers.AnyAsync())
        {
            await context.Customers.AddRangeAsync(SeedData.Customers);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedProductAsync(ApplicationContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(SeedData.Products);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedOrderAndOrderItemsAsync(ApplicationContext context)
    {
        if (!await context.Orders.AnyAsync())
        {
            await context.Orders.AddRangeAsync(SeedData.OrdersAndOrderItems);
            await context.SaveChangesAsync();
        }
    }
}
