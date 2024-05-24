using Models;

namespace Shared.IService;

public interface ICategoryervice
{
    Task<Category> CreateAsync(Category category);
    Task UpdateAsync(Category category);
    Task<Category?> GetByIdAsync(Guid tagId);
    Task DeleteAsync(Guid tagId);
    Task<IEnumerable<Category>> GetAllAsync();
    Task addTaskToTagAsync(Guid tagId, Guid taskId);
}