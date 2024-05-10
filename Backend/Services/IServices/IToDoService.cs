using EfcDatabase.Model;

namespace Services.IServices;

public interface IToDoService
{
    Task<ToDo> CreateAsync(ToDo todoToCreate);
    Task<ToDo?> GetByIdAsync(Guid todoId);
    Task UpdateAsync(ToDo todo);
    Task DeleteAsync(Guid todoId);
}