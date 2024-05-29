using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Shared;
using Shared.Dtos;

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

        [Function("SetTimeOffset")]
        public async Task<IActionResult> SetTimeOffset(
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
        [Function("GetClock")]
        public async Task<IActionResult> GetClock(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "clock/{clockId}")]
            HttpRequest req, Guid clockId)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var persistenceService = ServiceFactory.GetClockService();
                Clock? c = await persistenceService.GetClockById(clockId);
                if (c == null)
                {
                    throw new Exception("Bad Id");
                }
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
                // TODO Uncomment once the TCP Client to Server connection works and Server is on AzureVM
                var client = ServiceFactory.GetClient();
                if (await client.CheckClockIdAsync(clock.Id) == false)
                {
                    return new BadRequestObjectResult("Wrong clock id!");
                }
                Clock c = await persistenceService.CreateClockAsync(clock);
                ClockDTO clockDto = new ClockDTO()
                {
                    Id = c.Id,
                    Name = c.Name,
                    TimeOffset = c.TimeOffset,
                    UserId = c.OwnerId
                };
                return new OkResult();

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
                ClockDTO clock = JsonConvert.DeserializeObject<ClockDTO?>(requestBody);
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
        [Function("DeleteClock")]
        public async Task<IActionResult> DeleteClock(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "clock/{id}")]
            HttpRequest req, Guid id)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var persistenceService = ServiceFactory.GetClockService();
                await persistenceService.DeleteAsync(id);
                return new OkObjectResult("Clock deleted!");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult("Error creating clock!");
            }
        }

    }
}
