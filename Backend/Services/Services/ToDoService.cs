using Application.DAO;
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
}