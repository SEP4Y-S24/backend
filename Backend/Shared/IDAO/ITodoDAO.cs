using System.Linq.Expressions;
using Models;

namespace Shared.IDAO;

public interface ITodoDAO
{
    Task<Todo> CreateAsync(Todo todo);
    Task UpdateAsync(Todo todo);
    Task UpdateStatusByIdAsync(Guid todoId, Status status);
    Task<Todo?> GetByIdAsync(Guid todoId);
    Task DeleteAsync(Guid todoId);
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<IEnumerable<Todo>> GetAllByAsync(Expression<Func<Todo, bool>> filter);

}