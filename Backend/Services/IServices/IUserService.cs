using EfcDatabase.Model;

namespace Services.IServices;

public interface IUserService
{
    Task<User> CreateAsync(User userToCreate);
    Task<List<Clock>> GetClocksByUser(Guid id);
}