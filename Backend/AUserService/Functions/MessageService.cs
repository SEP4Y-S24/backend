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
    public class MessageService
    {
        private readonly ILogger<MessageService> _logger;

        public MessageService(ILogger<MessageService> logger)
        {
            _logger = logger;
        }

        [Function("MessageService")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
        [Function("SendMessage")]
        public async Task<IActionResult> SendMessage(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "messages")]
            HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var messageService = ServiceFactory.GetMessageService();
                var client =  ServiceFactory.GetClent();
               await client.SendTMAsync();
                SendMessageRequest sendMessageRequest = JsonConvert.DeserializeObject<SendMessageRequest>(requestBody);
                Message message = new Message()
                {
                    Id = Guid.NewGuid(),
                    DateOfCreation = DateTime.UtcNow,
                    SenderId = sendMessageRequest.userId,
                    ReceiverId = sendMessageRequest.receiverId,
                    ClockId = sendMessageRequest.clockId,
                    Body = sendMessageRequest.message
                };
                Message result = await messageService.SendMessage(message);
                SendMessageRequest response = new SendMessageRequest()
                {
                    userId = result.SenderId,
                    receiverId = result.ReceiverId,
                    clockId = result.ClockId,
                    message = result.Body
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
