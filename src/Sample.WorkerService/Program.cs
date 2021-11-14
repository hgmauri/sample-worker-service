using Sample.WorkerService.Core.Extensions;
using Sample.WorkerService.Workers;
using Serilog;

SerilogExtension.AddSerilog();
try
{
    Log.Information("Starting host");
    var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            services.AddCronJob<TimerJob>(c =>
            {
                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"*/1 * * * *";
            });
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