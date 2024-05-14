using EfcDatabase.Model;
using Microsoft.AspNetCore.Mvc;
using Services.IServices;
using WebApplication1.Dtos;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]

public class ClockController : ControllerBase
{
    private readonly IClockService _clockService;
    public ClockController(IClockService clockService)
    {
        this._clockService = clockService;
    }
    [HttpPatch("timeOffset")]
    public async Task<ActionResult> SetTimeOffsetAsync(long timeOffset, Guid id)
    {
        try
        {
            await _clockService.SetTimeZoneAsync(timeOffset,id);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateClockDTO dto)
    {
        try
        {
            Clock clock = new Clock()
            {
                Id= Guid.NewGuid(),
                OwnerId = dto.UserId,
                Name = dto.Name,
                TimeOffset = dto.TimeOffset,
            };
            Clock created = await _clockService.CreateClockAsync(clock);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateAsync(Guid id, CreateClockDTO createClock)
    {
        try
        {
            Clock clockToBeUpdated = new Clock()
            {
                Id = id,
                OwnerId = createClock.UserId,
                Name = createClock.Name,
                TimeOffset = createClock.TimeOffset,
            };
            await _clockService.UpdateClockAsync(clockToBeUpdated);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpDelete]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        try
        {
            await _clockService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}