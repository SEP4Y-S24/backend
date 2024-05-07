using EfcDatabase.Model;

namespace Application.DAO;

public interface IUserDAO
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<Clock> CreateAsync(Clock clock);
}