namespace Sample.WorkerService.Core.Events;

public class ClientSavedEvent
{
    public string? Name { get; set; }
    public string? Email { get; set; }
}