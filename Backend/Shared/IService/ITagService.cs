using Models;

namespace Shared.IService;

public interface ITagService
{
    Task<Tag> CreateAsync(Tag tag);
    Task UpdateAsync(Tag tag);
    Task<Tag?> GetByIdAsync(Guid tagId);
    Task DeleteAsync(Guid tagId);
    Task<IEnumerable<Tag>> GetAllAsync();
    Task addTaskToTagAsync(Guid tagId, Guid taskId);
    Task<IEnumerable<Tag>> GetAllByUserIdAsync(Guid userId);

}