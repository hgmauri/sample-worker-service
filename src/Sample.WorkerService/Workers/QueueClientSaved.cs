using MassTransit;
using Sample.WorkerService.Core.Events;

namespace Sample.WorkerService.Workers;

public class QueueClientSaved : IConsumer<ClientSavedEvent>
{
    private readonly ILogger<QueueClientSaved> _logger;

    public QueueClientSaved(ILogger<QueueClientSaved> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ClientSavedEvent> context)
    {
        _logger.LogInformation($"Received Client: {context.Message.Name}");

        return Task.CompletedTask;
    }
}