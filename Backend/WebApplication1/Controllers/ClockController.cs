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
    public async Task<ActionResult> UpdateAsync(char TimeZone, Guid id)
    {
        try
        {
            await _clockService.SetTimeZoneAsync(TimeZone,id);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}