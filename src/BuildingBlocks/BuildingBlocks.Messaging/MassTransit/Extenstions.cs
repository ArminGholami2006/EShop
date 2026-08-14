using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extenstions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = default)
    {
        services.AddMassTransit(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
            {
                configure.AddConsumers(assembly);
            }

            configure.UsingRabbitMq((context, configuratior) =>
            {
                configuratior.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:Username"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });
                configuratior.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
