using AAlarmService.DTOs;
using AAlarmService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AAlarmService.Functions
{
    public class AlarmServiceFunctions
    {
        private readonly ILogger<AlarmServiceFunctions> _logger;

        public AlarmServiceFunctions(ILogger<AlarmServiceFunctions> logger)
        {
            _logger = logger;
        }

        [Function("GetAllAlarms")]
        public async Task<IActionResult> GetAllAlarms([HttpTrigger(AuthorizationLevel.Function, "get", Route = "alarms/clocks/{clockId}")] HttpRequest req, Guid clockId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var alarmService = ServiceFactory.GetAlarmService();
            IEnumerable<Alarm> alarms =await alarmService.GetAllByClockAsync(clockId);
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
        
        [Function("GetAlarm")]
        public async Task<IActionResult> GetAlarm([HttpTrigger(AuthorizationLevel.Function, "get", Route = "alarm/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var alarmService = ServiceFactory.GetAlarmService();
            Alarm alarm =await alarmService.GetByIdAsync(id);
            AlarmDTO alarmDto = new AlarmDTO()
            {
                Id = alarm.Id,
                ClockId = alarm.ClockId,
                SetOffTime = alarm.SetOffTime,
                IsActive = alarm.IsActive,
                IsSnoozed = alarm.IsSnoozed
            };
            return new OkObjectResult(alarmDto);        
        }

    
        [Function("CreateAlarm")]
        public async Task<IActionResult> CreateAlarm([HttpTrigger(AuthorizationLevel.Function, "post", Route = "alarm")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            AlarmDTO alarm = JsonConvert.DeserializeObject<AlarmDTO>(requestBody);
            var alarmService = ServiceFactory.GetAlarmService();
            var clockDao = ServiceFactory.GetClockDAO();
            Clock? clock = await clockDao.GetByIdAsync(alarm.ClockId);
            if (clock == null)
            {
                return new BadRequestObjectResult("The given clock id is not found in the database");
            }
            alarm.SetOffTime.AddMinutes(clock.TimeOffset);
            Alarm alarmToCreate = new Alarm
            {
                Id = Guid.NewGuid(),
                ClockId = alarm.ClockId,
                SetOffTime = alarm.SetOffTime,
                IsActive = true,
                IsSnoozed = false
            };
            Alarm created = await alarmService.CreateAsync(alarmToCreate);
            AlarmDTO alarmDto = new AlarmDTO
            {
                Id = created.Id,
                ClockId = created.ClockId,
                SetOffTime = created.SetOffTime,
                IsActive = created.IsActive,
                IsSnoozed = created.IsSnoozed
            };
            return new OkObjectResult(alarmDto);        
        }

        [Function("ToggleAlarm")]
        public async Task<IActionResult> ToggleAlarm(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "alarm/{alarmId}/state")] HttpRequest req,
            Guid alarmId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ToggleAlarmDto state = JsonConvert.DeserializeObject<ToggleAlarmDto>(requestBody);
            if (alarmId.Equals(Guid.Empty))
            {
                return new BadRequestObjectResult("The given alarm id is not valid");
            }
            if(state.state == null)
            {
                return new BadRequestObjectResult("The given state is not valid");
            }
            var alarmService = ServiceFactory.GetAlarmService();
            
            Alarm alarm = await alarmService.ToggleAlarmAsync(alarmId, state.state);
            AlarmDTO alarmDto = new AlarmDTO
            {
                Id = alarm.Id,
                ClockId = alarm.ClockId,
                SetOffTime = alarm.SetOffTime,
                IsActive = alarm.IsActive,
                IsSnoozed = alarm.IsSnoozed
            };
            return new OkObjectResult(alarmDto);
        }

    }
}
