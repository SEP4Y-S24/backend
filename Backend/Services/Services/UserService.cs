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

    public async Task<User> GetByIdAsync(Guid userId)
    {
        try
        {
            return await _userDao.GetByIdAsync(userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateAsync(User user)
    {
        try
        {
            await _userDao.UpdateAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteAsync(Guid userId)
    {
        try
        {
            await _userDao.DeleteAsync(userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}