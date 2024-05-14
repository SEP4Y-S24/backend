namespace UserService.Dtos;

public class SendMessageRequest
{
    public string message { get; set; }
    public Guid receiverId { get; set; }
    public Guid clockId { get; set; }
    public Guid userId { get; set; }
}