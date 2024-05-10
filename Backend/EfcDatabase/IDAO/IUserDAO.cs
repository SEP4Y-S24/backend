using EfcDatabase.Model;

namespace EfcDatabase.IDAO;

public interface IUserDAO
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(Guid userId);
}