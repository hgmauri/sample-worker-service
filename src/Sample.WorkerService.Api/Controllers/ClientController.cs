using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.WorkerService.Core.Events;

namespace Sample.WorkerService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly ILogger<ClientController> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public ClientController(ILogger<ClientController> logger, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost()]
    public async Task<IActionResult> PostClient([FromBody] ClientSavedEvent client)
    {
        await _publishEndpoint.Publish(client);

        _logger.LogInformation($"Send client: {client.Name}");

        return Ok();
    }
}