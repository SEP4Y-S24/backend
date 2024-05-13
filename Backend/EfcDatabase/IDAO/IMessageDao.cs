using EfcDatabase.Model;

namespace EfcDatabase.IDAO;

public interface IMessageDao
{
    Task<Message> CreateAsync(Message message);
    Task<IEnumerable<Message>> GetAll();
    Task<IEnumerable<Message>> GetAllSentMessagesByUserIdAsync(Guid userId);
    Task<IEnumerable<Message>> GetAllReceivedMessagesByUserIdAsync(Guid userId);
    Task UpdateAsync(Message message);
    Task<Message?> GetByIdAsync(Guid messageId);
    Task DeleteAsync(Guid id);

}