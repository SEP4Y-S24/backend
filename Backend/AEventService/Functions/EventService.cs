using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;


namespace AEventService.Functions
{
    public class EventService
    {
        private readonly ILogger<EventService> _logger;

        public EventService(ILogger<EventService> logger)
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
