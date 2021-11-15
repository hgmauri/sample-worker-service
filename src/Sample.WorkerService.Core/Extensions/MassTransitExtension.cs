using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.WorkerService.Core.Extensions;

public static class MassTransitExtension
{
    public static void AddMassTransitPublisher(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(bus =>
        {
            bus.UsingRabbitMq((ctx, busConfigurator) =>
            {
                busConfigurator.Host(configuration.GetConnectionString("RabbitMq"));
            });
        });
        services.AddMassTransitHostedService();
    }
}