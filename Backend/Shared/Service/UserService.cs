using Isopoh.Cryptography.Argon2;
using Models;
using Shared.Dtos;
using Shared.IDAO;
using Shared.IService;

namespace Shared.Service;

public class UserService : IUserService
{
    private readonly IUserDAO _userDao;
    private readonly IClockDAO _clockDao;
    private readonly ITodoDAO _todoDao;


    public UserService(IUserDAO userDao, IClockDAO clockDao, ITodoDAO todoDao)
    {
        _userDao = userDao;
        _clockDao = clockDao;
        _todoDao = todoDao;
    }

    public async Task<User> CreateAsync(User userToCreate)
    {
        if (userToCreate == null)
        {
            throw new ArgumentNullException("The given user object is null");
        }
        User? user = await _userDao.GetByAsync(u => u.Email.Equals(userToCreate.Email));
        if (user != null)
        {
            throw new Exception("User with this email already exists!");
        }

        User created = await _userDao.CreateAsync(userToCreate);
        return created;
    }

    public async Task<User> Login(LoginRequest loginRequest)
    {
        User? user = await _userDao.GetByAsync(u => u.Email.Equals(loginRequest.Email));
        if (user == null)
        {
            throw new Exception("User with this email does not exist!");
        }

        if (!Argon2.Verify(user.PasswordHash, loginRequest.Password))
        {
            throw new ArgumentException("Password is incorrect!");
        }

        return user;

    }

    public async Task<User?> GetByIdAsync(Guid userId)
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
        if (user is null)
        {
            throw new ArgumentNullException("There is no user with this id!");
        }
        return user.Clocks.ToList();
    }

    /*   public async Task<Clock> AddClock(CreateClockDTO clock, Guid userId)
       {
           User? user = await _userDao.GetByIdAsync(clock.UserId);
           if (user == null)
           {
               throw new Exception($"User with id {clock.UserId} was not found.");
           }

           if (clock..Equals(Guid.Empty))
           {
               Clock ck = await _clockDao.GetByIdAsync(clock.Id);
               _clockDao.CreateAsync(ck);
           }
           Clock c = await _clockDao.GetByIdAsync(clock.Id);
           c.Owner = user;
           User u = await _userDao.GetByIdAsync(userId);
           u.Clocks.Add(c);
           await _userDao.UpdateAsync(u);
           return c;
       }*/

    /*  public async Task<ToDo> AddTodo(ToDo toDo, Guid userId)
      {
          if (toDo.Id.Equals(Guid.Empty))
          {
              _todoDao.CreateAsync(toDo);
          }

          ToDo c = await _todoDao.GetByIdAsync(toDo.Id);
          User u = await _userDao.GetByIdAsync(userId);
          u.Todos.Add(toDo);
          await _userDao.UpdateAsync(u);
          return toDo;
      }*/
}