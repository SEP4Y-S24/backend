using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CoupledClock.AlarmService
{
    public class AlarmServiceFunctions
    {
        private readonly ILogger<AlarmServiceFunctions> _logger;

        public AlarmServiceFunctions(ILogger<AlarmServiceFunctions> logger)
        {
            _logger = logger;
        }

        [Function("GetAllAlarms")]
        public IActionResult GetAllAlarms([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions! Workflow works");
        }
        
        [Function("GetAlarms")]
        public IActionResult GetAlarms([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions! Workflow works!");
        }
    }
}
