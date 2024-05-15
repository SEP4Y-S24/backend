using UserService.Dtos;
using UserService.IDAO;
using UserService.IServices;
using UserService.Model;

namespace UserService.Services;

public class UserServiceImpl : IUserService
{
    private readonly IUserDAO _userDao;
    private readonly IClockDAO _clockDao;
    private readonly IToDoDAO _toDoDao;


    public UserServiceImpl(IUserDAO userDao, IClockDAO clockDao,IToDoDAO todoDao)
    {
        _userDao = userDao;
        _clockDao = clockDao;
        _toDoDao = todoDao;
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

    public async Task<Clock> AddClock(CreateClockDto clock, Guid userId)
    {
        User? user = await _userDao.GetByIdAsync(clock.UserId);
        if (user == null)
        {
            throw new Exception($"User with id {clock.UserId} was not found.");
        }
        
        if (clock.Id.Equals(Guid.Empty))
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
    }

    public async Task<ToDo> AddTodo(ToDo toDo, Guid userId)
    {
        if (toDo.Id.Equals(Guid.Empty))
        {
            _toDoDao.CreateAsync(toDo);
        }

        ToDo c = await _toDoDao.GetByIdAsync(toDo.Id);
        User u = await _userDao.GetByIdAsync(userId);
        u.Todos.Add(toDo);
        await _userDao.UpdateAsync(u);
        return toDo;
    }
}