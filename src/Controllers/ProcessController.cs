using System.Collections;
using System.Net;
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
    static readonly string topicName = Environment.GetEnvironmentVariable("TOPIC_NAME") ?? "huichapan";
    static readonly string pubsubComponentDestiny = Environment.GetEnvironmentVariable("PUBSUB_COMPONENT_NAME") ?? "pubsub-resender-destiny";
    public ProcessController(ILogger<ProcessController> logger)
    {
        _logger = logger;
        //PrintAllEnvironmentVariables();
    }



    [HttpPost("/Process")]
    public async Task<IActionResult> Run([FromBody] object body)
    {
        _logger.LogInformation("Received request");
        _logger.LogInformation(body.ToString());

        try
        {
            await SendToOutput(body);
            return Ok();
        }
        catch (Exception ex)
        {
            //in case of error, log it and return 500, this way the message will be retried
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

        }
    }

    private async Task SendToOutput(object body)
    {
        try
        {
            string url = $"http://localhost:{daprPort}/v1.0/publish/{pubsubComponentDestiny}/{topicName}";
            _logger.LogInformation($"Sending to output ({url})");
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            Console.WriteLine($"Result:{response.ToString()} ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending to output");
        }
    }
}
