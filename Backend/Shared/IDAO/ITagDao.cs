using Models;

namespace Shared.IDAO;

public interface ITagDao
{
    Task<Tag> CreateAsync(Tag tag);
    Task UpdateAsync(Tag tag);
    Task<Tag?> GetByIdAsync(Guid tagId);
    Task DeleteAsync(Guid tagId);
    Task<IEnumerable<Tag>> GetAllAsync();
    Task<IEnumerable<Tag>> GetAllByUserIdAsync(Guid userId);



}