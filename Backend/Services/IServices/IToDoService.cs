using EfcDatabase.Model;

namespace Services.IServices;

public interface IToDoService
{
    Task<ToDo> CreateAsync(ToDo todoToCreate);
}