using Microsoft.AspNetCore.Mvc;
using Services.IServices;

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
    [HttpPatch]
    public async Task<ActionResult> UpdateAsync(long timeOffset, Guid id)
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
    public async Task<ActionResult> CreateAsync()
    {
        try
        {
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}