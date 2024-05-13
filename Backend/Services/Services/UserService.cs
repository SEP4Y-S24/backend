using EfcDatabase.IDAO;
using EfcDatabase.Model;
using Services.IServices;

namespace Services.Services;

public class UserService : IUserService
{
    private readonly IUserDAO _userDao;
    private readonly IClockDAO _clockDao;


    public UserService(IUserDAO userDao, IClockDAO clockDao)
    {
        _userDao = userDao;
        _clockDao = clockDao;
    }

    public async Task<User> CreateAsync(User userToCreate)
    {
        if (userToCreate == null)
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

    public async Task<List<Clock>> GetClocksByUser(Guid id)
    {
        User? user = await _userDao.GetByIdAsync(id);
        IEnumerable<Clock?> clocks = await _clockDao.GetAllByAsync(cl => cl.OwnerId.Equals(id));
        if (user is null)
        {
            throw new ArgumentNullException("There is no user with this id!");
        }

        if (clocks is null)
        {
            throw new ArgumentNullException("There are no clocks!");

        }


        return clocks.ToList();
    }
}