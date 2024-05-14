using System.Net;
using EfcDatabase.Model;
using Microsoft.AspNetCore.Mvc;
using Services.IServices;
using WebApplication1.Dtos;

namespace WebApplication1.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService _userService)
    {
        this._userService = _userService;
    }

    [HttpGet("{id}/clocks")]
    public async Task<ActionResult> GetAllById(Guid id)
    {
        try
        {
            List<Clock> clocks = _userService.GetClocksByUser(id).Result;
            if (clocks is null)
            {
                throw new ArgumentNullException("No available clocks!");
            }

            List<ClockDTO> clockDtos = new List<ClockDTO>();
            foreach (var clock in clocks)
            {
                ClockDTO clockDto = new ClockDTO()
                {
                    UserId = clock.Id,
                    TimeOffset = clock.TimeOffset,
                    Name = clock.Name,
                    Id = clock.Id
                };
                clockDtos.Add(clockDto);
            }
            return Ok(clockDtos);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

}