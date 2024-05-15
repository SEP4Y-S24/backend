using TodoServices.Model;

namespace TodoServices.IDAO;

public interface IUserDao
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid userId);
    
}