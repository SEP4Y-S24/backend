using System.Net;
using Microsoft.AspNetCore.Mvc;
using UserService.Dtos;
using UserService.IServices;
using UserService.Model;

namespace UserService.Controllers;
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
    
    [HttpPost("/clocks")]
    public async Task<ActionResult> AddClock(Guid userId, CreateClockDto clockToBeAdded)
    {
        try
        {

            Clock clock = _userService.AddClock(clockToBeAdded, userId).Result;
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    [HttpPost("/todo")]
    public async Task<ActionResult> AddTodo(Guid userId, ToDo toDoToBeAdded)
    {
        try
        {

            ToDo toDo = _userService.AddTodo(toDoToBeAdded, userId).Result;
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}