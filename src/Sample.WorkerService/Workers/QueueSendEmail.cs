using MassTransit;
using Sample.WorkerService.Core.Events;

namespace Sample.WorkerService.Workers;

public class QueueSendEmail : IConsumer<ClientSavedEvent>
{
    readonly ILogger<QueueSendEmail> _logger;

    public QueueSendEmail(ILogger<QueueSendEmail> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ClientSavedEvent> context)
    {
        _logger.LogInformation($"Email successfully sent to: {context.Message.Email}");

        return Task.CompletedTask;
    }
}
