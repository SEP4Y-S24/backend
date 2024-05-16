using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CoupledClock.AlarmService
{
    public class GetAllAlarms
    {
        private readonly ILogger<GetAllAlarms> _logger;

        public GetAllAlarms(ILogger<GetAllAlarms> logger)
        {
            _logger = logger;
        }

        [Function("GetAllAlarms")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
