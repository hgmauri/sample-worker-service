using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Timer = System.Timers.Timer;

namespace Sample.WorkerService.Core.Extensions;

public abstract class CronJobExtensions : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly CronExpression _expression;
    private readonly TimeZoneInfo _timeZoneInfo;

    protected CronJobExtensions(string cronExpression, TimeZoneInfo timeZoneInfo)
    {
        _expression = CronExpression.Parse(cronExpression);
        _timeZoneInfo = timeZoneInfo;
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        await ScheduleJob(cancellationToken);
    }

    public static TimeSpan GetDelay(string cronExpression)
    {
        var expression = CronExpression.Parse(cronExpression);
        var next = expression.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local);

        return next?.Subtract(DateTimeOffset.Now) ?? TimeSpan.FromHours(0);
    }

    protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
    {
        var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
        if (next.HasValue)
        {
            var delay = next.Value - DateTimeOffset.Now;
            if (delay.TotalMilliseconds <= 0)
            {
                await ScheduleJob(cancellationToken);
            }
            _timer = new System.Timers.Timer(delay.TotalMilliseconds);
            _timer.Elapsed += async (sender, args) =>
            {
                _timer.Dispose();
                _timer = null;

                if (!cancellationToken.IsCancellationRequested)
                {
                    await DoWork(cancellationToken);
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    await ScheduleJob(cancellationToken);
                }
            };
            _timer.Start();
        }
        await Task.CompletedTask;
    }

    public virtual Task<Task> DoWork(CancellationToken cancellationToken)
    {
        return Task.FromResult(Task.Delay(5000, cancellationToken));
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Stop();
        await Task.CompletedTask;
    }

    public virtual void Dispose()
    {
        _timer?.Dispose();
    }
}

public interface IScheduleConfig<T>
{
    string CronExpression { get; set; }
    TimeZoneInfo TimeZoneInfo { get; set; }
}

public class ScheduleConfig<T> : IScheduleConfig<T>
{
    public string? CronExpression { get; set; }
    public TimeZoneInfo? TimeZoneInfo { get; set; } = TimeZoneInfo.Local;
}

public static class ScheduledServiceExtensions
{
    public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options) where T : CronJobExtensions
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
        }
        var config = new ScheduleConfig<T>();
        options.Invoke(config);
        if (string.IsNullOrWhiteSpace(config.CronExpression))
        {
            throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
        }

        services.AddSingleton<IScheduleConfig<T>>(config);
        services.AddHostedService<T>();
        return services;
    }
}