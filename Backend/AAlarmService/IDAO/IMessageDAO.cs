using AlarmServices.Model;

namespace AAlarmServices.IDAO;

public interface IMessageDAO
{
    Task<Message> CreateAsync(Message message);
    Task<IEnumerable<Message>> GetAll();
    Task<IEnumerable<Message>> GetAllSentMessagesByUserIdAsync(Guid userId);
    Task<IEnumerable<Message>> GetAllReceivedMessagesByUserIdAsync(Guid userId);
    Task UpdateAsync(Message message);
    Task<Message?> GetByIdAsync(Guid messageId);
    Task DeleteAsync(Guid id);
}