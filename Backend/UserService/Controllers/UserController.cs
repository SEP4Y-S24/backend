using System.Net;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Mvc;
using UserService.Authorization;
using UserService.Dtos;
using UserService.IServices;
using UserService.Model;

namespace UserService.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtUtils _jwtUtils;
    public UserController(IUserService _userService, IJwtUtils _jwtUtils)
    {
        this._userService = _userService;
        this._jwtUtils = _jwtUtils;
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
    
    [HttpGet("{id}/send-messages")]
    public async Task<ActionResult> GetAllSendMessagesById(Guid id)
    {
        try
        {
            User user = _userService.GetByIdAsync(id).Result;
            if (user is null)
            {
                throw new ArgumentNullException("No such user!");
            }

            MessagesResponse response = new MessagesResponse();
            response.Messages = new List<SendMessageRequest>();
            foreach (var message in user.MessagesSent)
            {
                SendMessageRequest m = new SendMessageRequest()
                {
                    userId = user.Id,
                    receiverId = message.ReceiverId,
                    message = message.Body,
                    clockId = message.ClockId
                };
                response.Messages.Add(m);
            }

            response.UserID = user.Id;
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    [HttpGet("{id}/received-messages")]
    public async Task<ActionResult> GetAllRecievedMessagesById(Guid id)
    {
        try
        {
            User user = _userService.GetByIdAsync(id).Result;
            if (user is null)
            {
                throw new ArgumentNullException("No such user!");
            }

            MessagesResponse response = new MessagesResponse();
            response.Messages = new List<SendMessageRequest>();
            foreach (var message in user.MessagesRecieved)
            {
                SendMessageRequest m = new SendMessageRequest()
                {
                    userId = user.Id,
                    receiverId = message.ReceiverId,
                    message = message.Body,
                    clockId = message.ClockId
                };
                response.Messages.Add(m);
            }

            response.UserID = user.Id;
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    [HttpPost]
    public async Task<ActionResult> AddUser(CreateUserRequest userToBeAdded)
    {
        try
        {
            User user = new User()
            {
                Name = userToBeAdded.Name,
                Email = userToBeAdded.Email,
                AvatarId = userToBeAdded.AvatarId,
                PasswordHash = Argon2.Hash(userToBeAdded.Password)
            };
            User createdUSer = await _userService.CreateAsync(user);
            string token = _jwtUtils.GenerateJwtToken(createdUSer);
            return Ok(token);
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