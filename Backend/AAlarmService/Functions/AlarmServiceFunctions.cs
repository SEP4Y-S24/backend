using AAlarmService;
using AAlarmServices.DTOs;
using AlarmServices.Model;
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
        public async Task<IActionResult> GetAllAlarms([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var alarmService = ServiceFactory.GetAlarmService();
            IEnumerable<Alarm> alarms =await alarmService.GetAllAsync();
            AlarmsDTO alarmsDto = new AlarmsDTO();
            alarmsDto.Alarms = new List<AlarmDTO>();
            foreach (var alarm in alarms)
            {
                AlarmDTO alarmDto = new AlarmDTO()
                {
                    Id = alarm.Id,
                    ClockId = alarm.ClockId,
                    SetOffTime = alarm.SetOffTime,
                    IsActive = alarm.IsActive,
                    IsSnoozed = alarm.IsSnoozed
                };
                alarmsDto.Alarms.Add(alarmDto);
            }
            return new OkObjectResult(alarmsDto);
        }
        
        [Function("GetAlarms")]
        public IActionResult GetAlarms([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions! Workflow works!");
        }
    }
}
