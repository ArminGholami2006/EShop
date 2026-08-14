using Basket.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Extensions;

public static class ApplicationBuilderExtension
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        context.Database.Migrate();
    }
}
