using AClockService.Dtos;
using AClockService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        [Function("SetTimeZone")]
        public async Task<IActionResult> SetTimeZone(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "clock/{id}")]
            HttpRequest req, Guid id)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include
                };
                if (requestBody.Contains("timeOffset") == false)
                {
                    return new BadRequestObjectResult("Time offset is required!");
                }

                TimeOffsetRequest timeOffset =
                    JsonConvert.DeserializeObject<TimeOffsetRequest?>(requestBody, jsonSettings);
                var persistenceService = ServiceFactory.GetClockService();
                await persistenceService.SetTimeZoneAsync(timeOffset.TimeOffset, id);
                return new OkObjectResult(
                    $"Time offset {timeOffset.TimeOffset} for clock with id: {id} succefully set!");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult("Error setting time offset!");
            }
        }

        [Function("CreateClock")]
        public async Task<IActionResult> CreateClock(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "clock")]
            HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                CreateClockDTO clock = JsonConvert.DeserializeObject<CreateClockDTO?>(requestBody);
                var persistenceService = ServiceFactory.GetClockService();
                Clock c = await persistenceService.CreateClockAsync(clock);
                ClockDTO clockDto = new ClockDTO()
                {
                    Id = c.Id,
                    Name = c.Name,
                    TimeOffset = c.TimeOffset,
                    UserId = c.OwnerId
                };
                return new OkObjectResult(clockDto);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult("Error creating clock!");
            }
        }

        [Function("UpdateClock")]
        public async Task<IActionResult> UpdateClock(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "clock/{id}")]
            HttpRequest req, Guid id)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                CreateClockDTO clock = JsonConvert.DeserializeObject<CreateClockDTO?>(requestBody);
                var persistenceService = ServiceFactory.GetClockService();
                Clock c = await persistenceService.UpdateClockAsync(clock, id);
                ClockDTO clockDto = new ClockDTO()
                {
                    Id = c.Id,
                    Name = c.Name,
                    TimeOffset = c.TimeOffset,
                    UserId = c.OwnerId
                };
                return new OkObjectResult(clockDto);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult("Error creating clock!");
            }
        }
    }
}
