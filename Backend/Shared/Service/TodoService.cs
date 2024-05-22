using Models;
using Shared.IDAO;
using Shared.IServices;
namespace Shared.Services;

public class TodoService: ITodoService
{
    private readonly ITodoDAO _todoDao;
    private readonly IUserDAO _userDao;
    private readonly ITagDao _tagDao;

    public TodoService(ITodoDAO todoDao, IUserDAO userDao, ITagDao tagDao)
    {
        _todoDao = todoDao;
        _userDao = userDao;
        _tagDao = tagDao;
    }
    
    public async Task<Todo> CreateAsync(Todo todoToCreate)
    {
        User? user = await _userDao.GetByIdAsync(todoToCreate.UserId);
        if (user==null)
        {
            throw new Exception($"User with id {todoToCreate.UserId} was not found.");
        }

        Todo todo = new Todo(todoToCreate.UserId, todoToCreate.Name, todoToCreate.Description, todoToCreate.Deadline,
            todoToCreate.Status);
        Todo created = await _todoDao.CreateAsync(todo);
        return created;
    }

    public async Task<Todo?> GetByIdAsync(Guid todoId)
    {
        try
        {
            return await _todoDao.GetByIdAsync(todoId);
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
            await _todoDao.UpdateAsync(todo);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateStatusByIdAsync(Guid todoId, Status status)
    {
        try
        {
            await _todoDao.UpdateStatusByIdAsync(todoId, status);
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
            await _todoDao.DeleteAsync(todoId);
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
            return await _todoDao.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Todo>> FilterByTags(List<Tag> tags, Guid userId)
    {
        IEnumerable<Tag> t = await _tagDao.GetAllAsync();
        List<Todo> todos = new List<Todo>();
        foreach (var tag in t)
        {
            if (tags.Where(ta=>ta.Name.Equals(tag.Name)).Any())
            {
                foreach (var todo in tag.Todos)
                {
                    if (todo.UserId == userId)
                    {
                        todos.Add(todo);
                    }
                }
            }
       }
        todos = todos.Distinct().ToList();
        return todos;
    }
}