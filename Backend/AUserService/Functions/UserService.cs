using System.Security.Claims;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Shared;
using Shared.Dtos;
using Shared.DTOs;
using Shared.Helpers;
using Shared.Migrations;

namespace AUserService.Functions
{
     public class UserService
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        [Function("UserService")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
        
        [ClaimRequirement(ClaimTypes.Role, "User")]
        [Authorize]
        [Function("GetAllClocks")]
        public async Task<IActionResult> GetAllClocks(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{userId}/clocks")]
            HttpRequest req, Guid userId)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                var userService = ServiceFactory.GetUserService();
                IEnumerable<Clock> clocks = await userService.GetClocksByUser(userId);
                if (clocks is null)
                {
                    throw new ArgumentNullException("No available clocks!");
                }

                List<ClockDTO> clockDtos = new List<ClockDTO>();
                foreach (var clock in clocks)
                {
                    ClockDTO clockDto = new ClockDTO()
                    {
                        UserId = clock.OwnerId,
                        TimeOffset = clock.TimeOffset,
                        Name = clock.Name,
                        Id = clock.Id
                    };
                    clockDtos.Add(clockDto);
                }

                return new OkObjectResult(clockDtos);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e);
                return new BadRequestObjectResult("Error getting clock!");
            }
        }

        [ClaimRequirement(ClaimTypes.Role, "User")]
        [Function("GetAllMessages")]
        public async Task<ActionResult> GetAllMessagesById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{userId}/messages")]
            HttpRequest req, Guid userId, [FromQuery] string? activity)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                var userService = ServiceFactory.GetUserService();
                User? user = await userService.GetByIdAsync(userId);
                if (user is null)
                {
                    throw new ArgumentNullException("No such user!");
                }

                if (activity == null)
                {
                    throw new ArgumentNullException("No correct activity!");
                }

                MessagesResponse response = new MessagesResponse();
                response.Messages = new List<SendMessageRequest>();
                List<Message> ms = new List<Message>();
                if (activity.ToLower().Equals("sent"))
                {
                    ms = user.MessagesSent.ToList();
                }

                if (activity.ToLower().Equals("received"))
                {
                    ms = user.MessagesRecieved.ToList();
                }

                foreach (var message in ms)
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
                return new OkObjectResult(response);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        [Function("Regiter")]
        public async Task<IActionResult> Regiter(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users")]
            HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var userService = ServiceFactory.GetUserService();
                CreateUserRequest createUserRequest = JsonConvert.DeserializeObject<CreateUserRequest>(requestBody);
                User user = new User()
                {
                    Name = createUserRequest.Name,
                    Email = createUserRequest.Email,
                    AvatarId = createUserRequest.AvatarId,
                    PasswordHash = Argon2.Hash(createUserRequest.Password)
                };
                User createdUSer = await userService.CreateAsync(user);
                var jwtUtils = ServiceFactory.GetJwtUtils();

                string token = jwtUtils.GenerateJwtToken(createdUSer);

                UserDto userDto = new UserDto()
                {
                    UserId = createdUSer.Id,
                    Name = createdUSer.Name,
                    Email = createdUSer.Email,
                    AvatarId = createdUSer.AvatarId
                };
                LoginResponse response = new LoginResponse()
                {
                    Token = token,
                    User = userDto
                };
                return new OkObjectResult(response);

            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        [Function("Login")]
        public async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/login")]
            HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var userService = ServiceFactory.GetUserService();
                var jwtUtils = ServiceFactory.GetJwtUtils();
                LoginRequest loginRequest = JsonConvert.DeserializeObject<LoginRequest>(requestBody);
                User createdUSer = await userService.Login(loginRequest);
                string token = jwtUtils.GenerateJwtToken(createdUSer);
                UserDto userDto = new UserDto()
                {
                    UserId = createdUSer.Id,
                    Name = createdUSer.Name,
                    Email = createdUSer.Email,
                    AvatarId = createdUSer.AvatarId
                };
                LoginResponse response = new LoginResponse()
                {
                    Token = token,
                    User = userDto
                };
                return new OkObjectResult(response);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        [Function("AddContact")]
        public async Task<IActionResult> AddContact(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/contact")]
            HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var service = ServiceFactory.GetContactService();
                AddContactDto contactDto = JsonConvert.DeserializeObject<AddContactDto>(requestBody);
                Contact contact = new Contact()
                {
                    id = Guid.NewGuid(),
                    Email1 = contactDto.Email1,
                    Email2 = contactDto.Email2,
                };
                await service.CreateAsync(contact);
              //  u.Contacts.Add(u2);
                return new OkObjectResult("Contact added!");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
        [Function("DeleteContact")]
        public async Task<IActionResult> DeleteContact(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "users/contact")]
            HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var service = ServiceFactory.GetContactDao();
                AddContactDto contactDto = JsonConvert.DeserializeObject<AddContactDto>(requestBody);
                await service.DeleteAsync(contactDto.Email1,contactDto.Email2);
                //  u.Contacts.Add(u2);
                return new OkObjectResult("Contact deleted!");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
        [Function("GetContacts")]
        public async Task<IActionResult> GetContacts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{userEmail}/contact")]
            HttpRequest req, string userEmail)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                var service = ServiceFactory.GetContactService();
                IEnumerable<User> users =  await service.GetAllContactsByUserIdAsync(userEmail);
                UsersDto usersDto = new UsersDto();
                usersDto.Users = new List<UserDto>();
                foreach (var u in users)
                {
                    UserDto us = new UserDto()
                    {
                        AvatarId = u.AvatarId,
                        Email = u.Email,
                        Name = u.Name,
                        UserId = u.Id
                    };
                    usersDto.Users.Add(us);
                }
                //  u.Contacts.Add(u2);
                return new OkObjectResult(usersDto);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}