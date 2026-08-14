using Refit;
using Shopping.Web.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddRazorPages();

services.AddRefitClient<ICatalogService>()
    .AddRefitClient<IBasketService>()
    .AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(clinet =>
    {
        clinet.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
