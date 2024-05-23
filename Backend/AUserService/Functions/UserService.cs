using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Shared;
using Shared.Dtos;

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
    }
}
