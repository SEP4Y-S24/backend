using EfcDatabase.Model;

namespace Services.IServices;

public interface IMessageService
{
    Task<Message> SendMessageAsync(Message message);
    Task<IEnumerable<Message>> GetAllSentMessagesByUserIdAsync(Guid userId);
    Task<IEnumerable<Message>> GetAllReceivedMessagesByUserIdAsync(Guid userId);
}