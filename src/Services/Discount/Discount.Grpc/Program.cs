using Discount.Grpc.Infrastructure.Persistence;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<DiscountService>();

app.Run();
