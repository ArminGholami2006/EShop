using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

services.AddRateLimiter(configuration =>
{
    configuration.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

var app = builder.Build();

app.UseRateLimiter();

app.MapReverseProxy();

app.Run();
