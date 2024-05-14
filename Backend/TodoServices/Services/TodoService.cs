using TodoServices.IDAO;
using TodoServices.IServices;
using TodoServices.Model;

namespace TodoServices.Services;

public class TodoService: ITodoService
{
    private readonly ITodoDao _toDoDao;
    private readonly IUserDao _userDao;

    public TodoService(ITodoDao toDoDao, IUserDao userDao)
    {
        _toDoDao = toDoDao;
        _userDao = userDao;
    }
    
    public async Task<Todo> CreateAsync(Todo todoToCreate)
    {
        User? user = await _userDao.GetByIdAsync(todoToCreate.UserId);
        if (user==null)
        {
            throw new Exception($"User with id {todoToCreate.UserId} was not found.");
        }

        Todo todo = new Todo(user, todoToCreate.Name, todoToCreate.Description, todoToCreate.Deadline,
            todoToCreate.Status);
        Todo created = await _toDoDao.CreateAsync(todo);
        return created;
    }

    public async Task<Todo?> GetByIdAsync(Guid todoId)
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

    public async Task UpdateAsync(Todo todo)
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

    public async Task<IEnumerable<Todo>> GetAllAsync()
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