using MassTransit;
using Sample.WorkerService.Core.Extensions;
using Sample.WorkerService.Workers;
using Serilog;

SerilogExtension.AddSerilog();

try
{
    Log.Information("Starting host");
    var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<QueueClientSaved>();
                x.AddConsumer<QueueSendEmail>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(hostContext.Configuration.GetConnectionString("RabbitMq"));
                    cfg.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService(true);

            services.AddCronJob<TimerJob>(c => c.CronExpression = @"*/1 * * * *");
        })
        .UseSerilog()
        .Build();

    await host.RunAsync();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}