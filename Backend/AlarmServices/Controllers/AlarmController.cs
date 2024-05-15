using AlarmServices.DTOs;
using AlarmServices.IService;
using AlarmServices.Model;
using Microsoft.AspNetCore.Mvc;

namespace AlarmServices.Controllers;

[ApiController]
[Route("[controller]")]
public class AlarmController: ControllerBase
{
    private readonly IAlarmService _alarmService;

    public AlarmController(IAlarmService alarmService)
    {
        _alarmService = alarmService;
    }

    [HttpPost]
    public async Task<ActionResult<AlarmDTO>> CreateAsync(CreateAlarmDTO alarm)
    {
        try
        {
            Alarm alarmToCreate = new Alarm
            {
                Id = Guid.NewGuid(),
                ClockId = alarm.ClockId,
                SetOffTime = alarm.SetOffTime,
                IsActive = alarm.IsActive,
                IsSnoozed = alarm.IsSnoozed
            };
            Alarm created = await _alarmService.CreateAsync(alarmToCreate);
            AlarmDTO alarmDto = new AlarmDTO
            {
                Id = created.Id,
                ClockId = created.ClockId,
                SetOffTime = created.SetOffTime,
                IsActive = created.IsActive,
                IsSnoozed = created.IsSnoozed
            };
            return Created($"/alarms/{created.Id}", alarmDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<AlarmDTO>> UpdateAsync(AlarmDTO alarm)
    {
        try
        {
            Alarm alarmToUpdate = new Alarm()
            {
                Id = alarm.Id,
                ClockId = alarm.ClockId,
                SetOffTime = alarm.SetOffTime,
                IsActive = alarm.IsActive,
                IsSnoozed = alarm.IsSnoozed
            };

            await _alarmService.UpdateAsync(alarmToUpdate);
            AlarmDTO alarmDto = new AlarmDTO()
            {
                Id = Guid.NewGuid(),
                ClockId = alarm.ClockId,
                SetOffTime = alarm.SetOffTime,
                IsActive = alarm.IsActive,
                IsSnoozed = alarm.IsSnoozed
            };
            return Ok(alarmDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Alarm>>> GetAllAsync()
    {
        try
        {
            IEnumerable<Alarm?> list= await _alarmService.GetAllAsync();
            if (!list.Any())
            {
                return NotFound(); 
            }
            return Ok(list);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Alarm>> GetByIdAsync(Guid id)
    {
        try
        {
            Alarm? alarm = await _alarmService.GetByIdAsync(id);
            if (alarm == null)
            {
                return NotFound();
            }
        
            return Ok(alarm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{alarmId}")]
    public async Task<ActionResult> DeleteAsync(Guid alarmId)
    {
        try
        {
            await _alarmService.DeleteAsync(alarmId);
            return NoContent();  // this is a better practice than Ok()
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch("{alarmId}/enable")]
    public async Task<ActionResult> EnableAsync(Guid alarmId)
    {
        try
        {
            await _alarmService.EnableAlarmAsync(alarmId);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("{alarmId}/disable")]
    public async Task<ActionResult> DisableAsync(Guid alarmId)
    {
        try
        {
            await _alarmService.DisableAlarmAsync(alarmId);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}