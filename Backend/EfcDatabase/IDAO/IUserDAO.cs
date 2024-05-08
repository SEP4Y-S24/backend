using EfcDatabase.Model;

namespace EfcDatabase.IDAO;

public interface IUserDAO
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<Clock> CreateAsync(Clock clock);
}