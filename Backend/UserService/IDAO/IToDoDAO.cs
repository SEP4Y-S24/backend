using UserService.Model;

namespace UserService.IDAO;

public interface IToDoDAO
{
    Task<ToDo> CreateAsync(ToDo todo);
    Task<ToDo?> GetByIdAsync(Guid todoId);
}