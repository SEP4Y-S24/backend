using EfcDatabase.IDAO;
using EfcDatabase.Model;
using Services.IServices;

namespace Services.Services;

public class UserService:IUserService
{
    private readonly IUserDAO _userDao;

    public UserService(IUserDAO userDao)
    {
        _userDao = userDao;
    }
    public async Task<User> CreateAsync(User userToCreate)
    {
        if (userToCreate==null)
        {
            throw new ArgumentNullException("The given user object is null");
        }

        User created = await _userDao.CreateAsync(userToCreate);
        return created;
    }
}