using Models;

namespace Shared.IService;

public interface ITodoService
{
    Task<Todo> CreateAsync(Todo todoToCreate);
    Task<Todo?> GetByIdAsync(Guid todoId);
    Task UpdateAsync(Todo todo);
    Task UpdateStatusByIdAsync(Guid todoId, Status status);
    Task DeleteAsync(Guid todoId);
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<IEnumerable<Todo>> FilterByTags(List<Tag> tags, Guid userId);
}