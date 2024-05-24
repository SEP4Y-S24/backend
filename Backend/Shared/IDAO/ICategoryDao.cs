using Models;

namespace Shared.IDAO;

public interface ICategoryDao
{
    Task<Category> CreateAsync(Category category);
    Task UpdateAsync(Category category);
    Task<Category?> GetByIdAsync(Guid tagId);
    Task DeleteAsync(Guid tagId);
    Task<IEnumerable<Category>> GetAllAsync();

}