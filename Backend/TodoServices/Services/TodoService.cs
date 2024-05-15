using Microsoft.AspNetCore.Identity;
using TodoServices.IDAO;
using TodoServices.IServices;
using TodoServices.Model;

namespace TodoServices.Services;

public class TodoService: ITodoService
{
    private readonly ITodoDao _toDoDao;
    private readonly IUserDao _userDao;
    private readonly ITagDao _tagDao;

    public TodoService(ITodoDao toDoDao, IUserDao userDao, ITagDao tagDao)
    {
        _toDoDao = toDoDao;
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

    public async Task UpdateStatusByIdAsync(Guid todoId, Status status)
    {
        try
        {
            await _toDoDao.UpdateStatusByIdAsync(todoId, status);
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

    public async Task<IEnumerable<Todo>> FilterByTags(List<Tag> tags)
    {
        IEnumerable<Tag> t = await _tagDao.GetAllAsync();
        List<Todo> todos = new List<Todo>();
        foreach (var tag in t)
        {
            if (tags.Where(ta=>ta.name.Equals(tag.name)).Any())
            {
                foreach (var todo in tag.Todos)
                {
                    todos.Add(todo);
                }
            }
       }
        todos = todos.Distinct().ToList();
        return todos;
    }
}