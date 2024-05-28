using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Shared;
using Shared.Dtos;
using Shared.DTOs;

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

    
    [Function("CreateMeasurement")]
    public async Task<IActionResult> CreateMeasurement(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        CreateMeasurementDto measurement = JsonConvert.DeserializeObject<CreateMeasurementDto?>(requestBody);
        var persistenceService = ServiceFactory.GetMeasurementService();
        Measurement m = new Measurement()
        {
            Value = measurement.Value,
            ClockId = measurement.ClockId,
            Type = measurement.Type,
        };
        Measurement c = await persistenceService.CreateAsync(m);

        return new OkObjectResult("Welcome to Azure Functions!");
    }

    [Function("GetAvarageMeasurement")]
    public async Task<IActionResult> GetAvarageMeasurement(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "measurements/{clockId}/avarage")] HttpRequest req,
        Guid clockId, [FromQuery] string? type)
    {
        try
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CreateMeasurementDto measurement = JsonConvert.DeserializeObject<CreateMeasurementDto?>(requestBody);
            var persistenceService = ServiceFactory.GetMeasurementService();
            MeasurementType m;
            switch (type.ToLower())
            {
                case "co2":
                    m = MeasurementType.CO2;
                    break;
                case "humidity":
                    m = MeasurementType.Humidity;
                    break;
                case "temperature":
                    m = MeasurementType.Temperature;
                    break;
                case "aircondition":
                    m = MeasurementType.AirCondition;
                    break;
                default:
                    return new BadRequestObjectResult("Measurement type not found!");
            }

            double result = await persistenceService.GetAvarageByClockTodayAsync(clockId, m);
            return  new OkObjectResult(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new BadHttpRequestException(e.Message);
        }
    }
    [Function("GetMeasurement")]
    public async Task<IActionResult> GeMeasurement(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "measurements/{clockId}")] HttpRequest req,
        Guid clockId, [FromQuery] string? type)
    {
        try
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CreateMeasurementDto measurement = JsonConvert.DeserializeObject<CreateMeasurementDto?>(requestBody);
            var persistenceService = ServiceFactory.GetMeasurementService();
            MeasurementType m;
            switch (type.ToLower())
            {
                case "co2":
                    m = MeasurementType.CO2;
                    break;
                case "humidity":
                    m = MeasurementType.Humidity;
                    break;
                case "temperature":
                    m = MeasurementType.Temperature;
                    break;
                case "aircondition":
                    m = MeasurementType.AirCondition;
                    break;
                default:
                    return new BadRequestObjectResult("Measurement type not found!");
            }

            double result = await persistenceService.GetLAstByClockAsync(clockId, m);
            return  new OkObjectResult(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new BadHttpRequestException(e.Message);
        }
    }

}