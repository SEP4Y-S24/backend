

using Shared.DTOs;

namespace Shared.Dtos;

public class MessagesResponse
{
    public Guid UserID { get; set; }
    public List<MessageResponseDTO> Messages { get; set; }
}