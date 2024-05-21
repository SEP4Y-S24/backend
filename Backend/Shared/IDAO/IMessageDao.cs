using Models;

namespace Shared.IDAO;

public interface IMessageDao
{
    Task<Message> CreateAsync(Message message);
    Task<IEnumerable<Message>> GetAll();
    Task UpdateAsync(Message message);
    Task<Message?> GetByIdAsync(Guid messageId);
    Task DeleteAsync(Guid id);

}