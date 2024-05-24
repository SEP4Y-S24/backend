using System.Linq.Expressions;
using Models;

namespace Shared.IDAO;

public interface IEventDAO
{
    Task<Event> CreateAsync(Event entity);
    Task UpdateAsync(Event entity);
    Task UpdateStatusByIdAsync(Guid eventId, Status status);
    Task<Event?> GetByIdAsync(Guid eventId);
    Task DeleteAsync(Guid eventId);
    Task<IEnumerable<Event>> GetAllAsync();
    Task<IEnumerable<Event>> GetAllByUserIdAsync(Guid eventId);
    Task<IEnumerable<Event>> GetAllByAsync(Expression<Func<Event, bool>> filter);

}