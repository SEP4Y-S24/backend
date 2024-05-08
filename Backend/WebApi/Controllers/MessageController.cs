using EfcDatabase.IDAO;
using EfcDatabase.Model;
using Microsoft.AspNetCore.Mvc;
using Services.IServices;
using WebApplication1.Dtos;

namespace WebApplication1.Controllers;
[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    public MessageController(IMessageService _messageService)
    {
        this._messageService = _messageService;
    }

    [HttpPost]
    public async Task<ActionResult> UpdateAsync(SendMessageRequest dto)
    {
        try
        {
            Message message = new Message()
            {
                Id = Guid.NewGuid(),
                DateOfCreation = DateTime.UtcNow,
                SenderId = dto.userId,
                ReceiverId = dto.receiverId,
                ClockId = dto.clockId,
                Body = dto.message
            };
            Message result = await _messageService.SendMessage(message);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}