using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AMeasurementService;

public class MeasurementService
{
    private readonly ILogger _logger;

    public MeasurementService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<MeasurementService>();
    }

    [Function("ClockService")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}