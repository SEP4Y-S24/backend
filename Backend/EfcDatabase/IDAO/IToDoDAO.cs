using EfcDatabase.Model;

namespace Application.DAO;

public interface IToDoDAO
{
    Task<ToDo> CreateAsync(ToDo todo);
    Task UpdateAsync(Guid todoId);
    Task<ToDo?> GetByIdAsync(Guid todoId);
    Task DeleteAsync(Guid todoId);
}