using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TelemetryResender.Controllers;

[ApiController]
[Route("[controller]")]
public class ProcessController : ControllerBase
{
    private readonly ILogger<ProcessController> _logger;
    static readonly HttpClient client = new HttpClient();
    static readonly string daprPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500";
    static readonly string topicName = Environment.GetEnvironmentVariable("TOPIC_NAME") ?? "undefined_topic";
    static readonly string pubsubComponentName = Environment.GetEnvironmentVariable("PUBSUB_COMPONENT_NAME") ?? "undefined_pubsub_component_name";
    public ProcessController(ILogger<ProcessController> logger)
    {
        _logger = logger;
    }

    public async Task Run([FromBody] object body)
    {
        _logger.LogInformation("Received request");
        _logger.LogInformation(body.ToString());

        await SendToOutput(body);
    }

    private async Task SendToOutput(object body)
    {
        string url = $"http://localhost:{daprPort}/v1.0/{pubsubComponentName}/{topicName}";
        _logger.LogInformation($"Sending to output ({url})");
        var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        Console.WriteLine($"Result:{response.ToString()} ");

    }
}
