using EfcDatabase.Model;

namespace EfcDatabase.IDAO;

public interface IToDoDAO
{
    Task<ToDo> CreateAsync(ToDo todo);
    Task UpdateAsync(ToDo todo);
    Task<ToDo?> GetByIdAsync(Guid todoId);
    Task DeleteAsync(Guid todoId);
}