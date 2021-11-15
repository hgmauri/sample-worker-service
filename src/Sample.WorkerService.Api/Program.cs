using Sample.WorkerService.Core.Extensions;
using Serilog;

SerilogExtension.AddSerilog();

try
{
    Log.Information("Starting host");
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddRouting(options => options.LowercaseUrls = true);

    builder.Services.AddMassTransitPublisher(builder.Configuration);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    await app.RunAsync();
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