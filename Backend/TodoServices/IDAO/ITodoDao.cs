using TodoServices.Model;

namespace TodoServices.IDAO;

public interface ITodoDao
{
    Task<Todo> CreateAsync(Todo todo);
    Task UpdateAsync(Todo todo);
    Task UpdateStatusByIdAsync(Guid todoId, Status status);
    Task<Todo?> GetByIdAsync(Guid todoId);
    Task DeleteAsync(Guid todoId);
    Task<IEnumerable<Todo>> GetAllAsync();
}