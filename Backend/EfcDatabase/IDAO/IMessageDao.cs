using EfcDatabase.Model;

namespace Application.DAO;

public interface IMessageDao
{
    Task<Message> CreateAsync(Message clock);
    Task<IEnumerable<Message>> GetAsync();
    Task UpdateAsync(Message clock);
    Task<Message?> GetByIdAsync(Guid messageId);
    Task DeleteAsync(Guid id);

}