using Models;

namespace Shared.IService;

public interface IEventService
{
    Task<Event> CreateAsync(Event eventToCreate);
    Task<Event?> GetByIdAsync(Guid eventId);
    Task UpdateAsync(Event ev);
    Task UpdateStatusByIdAsync(Guid eventId, Status status);
    Task DeleteAsync(Guid eventId);
    Task<IEnumerable<Event>> GetAllAsync();
    Task<IEnumerable<Event>> GetAllByUserIdAsync(Guid userId);
    Task<IEnumerable<Event>> FilterByTags(List<Tag> tags, Guid userId);

}