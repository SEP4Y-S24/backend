using EfcDatabase.Model;

namespace Services.IServices;

public interface IUserService
{
    Task<User> CreateAsync(User userToCreate);
}