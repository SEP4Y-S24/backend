using EfcDatabase.IDAO;
using EfcDatabase.Model;
using Services.IServices;

namespace Services.Services;

public class ToDoService: IToDoService
{
    private readonly IToDoDAO _toDoDao;
    private readonly IUserDAO _userDao;

    public ToDoService(IToDoDAO toDoDao, IUserDAO userDao)
    {
        _toDoDao = toDoDao;
        _userDao = userDao;
    }
    
    public async Task<ToDo> CreateAsync(ToDo todoToCreate)
    {
        User? user = await _userDao.GetByIdAsync(todoToCreate.UserId);
        if (user==null)
        {
            throw new Exception($"User with id {todoToCreate.UserId} was not found.");
        }

        ToDo todo = new ToDo(user, todoToCreate.Name, todoToCreate.Description, todoToCreate.Deadline,
            todoToCreate.Status);
        ToDo created = await _toDoDao.CreateAsync(todo);
        return created;
    }

    public async Task<ToDo?> GetByIdAsync(Guid todoId)
    {
        try
        {
            return await _toDoDao.GetByIdAsync(todoId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateAsync(ToDo todo)
    {
        try
        {
            await _toDoDao.UpdateAsync(todo);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteAsync(Guid todoId)
    {
        try
        {
            await _toDoDao.DeleteAsync(todoId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<ToDo>> GetAllAsync()
    {
        try
        {
          return await _toDoDao.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}