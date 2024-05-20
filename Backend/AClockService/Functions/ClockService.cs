using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AClockService.Functions
{
    public class ClockService
    {
        private readonly ILogger<ClockService> _logger;

        public ClockService(ILogger<ClockService> logger)
        {
            _logger = logger;
        }

        [Function("ClockService")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
