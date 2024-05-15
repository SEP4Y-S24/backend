using UserService.Model;

namespace UserService.IDAO;

public interface IUserDAO
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid userId);
    Task DeleteClock(Guid clockId, Guid userId);

}