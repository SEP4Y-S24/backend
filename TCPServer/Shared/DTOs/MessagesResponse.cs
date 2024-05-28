

namespace Shared.Dtos;

public class MessagesResponse
{
    public Guid UserID { get; set; }
    public List<SendMessageRequest> Messages { get; set; }
}