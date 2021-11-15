using Sample.WorkerService.Core.Extensions;

namespace Sample.WorkerService.Workers;

public class TimerJob : CronJobExtensions
{
    public TimerJob(IScheduleConfig<TimerJob> config)
        : base(config.CronExpression, config.TimeZoneInfo)
    {
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        Serilog.Log.Information($"Worker {nameof(TimerJob)} iniciado.");
        return base.StartAsync(cancellationToken);
    }

    public override async Task<Task> DoWork(CancellationToken cancellationToken)
    {
        try
        {
            Serilog.Log.Information($"{nameof(TimerJob)} executado com sucesso às {DateTime.Now:T}");
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, $"Erro ao executar o {nameof(TimerJob)}");
        }
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Serilog.Log.Information($"{nameof(TimerJob)} finalizado!");
        return base.StopAsync(cancellationToken);
    }
}
