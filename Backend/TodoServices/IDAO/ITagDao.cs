using TodoServices.Model;

namespace TodoServices.IDAO;

public interface ITagDao
{
    Task<Tag> CreateAsync(Tag tag);
    Task UpdateAsync(Tag tag);
    Task<Tag?> GetByIdAsync(Guid tagId);
    Task DeleteAsync(Guid tagId);
    Task<IEnumerable<Tag>> GetAllAsync();

}