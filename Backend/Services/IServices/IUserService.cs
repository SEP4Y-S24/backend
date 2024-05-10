using EfcDatabase.Model;

namespace Services.IServices;

public interface IUserService
{
    Task<User> CreateAsync(User userToCreate);
    Task<User> GetByIdAsync(Guid userId);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid userId);
}