namespace Shared.DTOs;

public class MessageResponseDTO
{
    public string message { get; set; }
    public Guid receiverId { get; set; }
    public Guid clockId { get; set; }
    public Guid userId { get; set; }
    public int senderAvatarId { get; set; }
    public int receiverAvatarId { get; set; }
    public string senderEmail { get; set; }
    public string receiverEmail { get; set; }
}