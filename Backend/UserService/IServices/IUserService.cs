using UserService.Dtos;
using UserService.Model;

namespace UserService.IServices;

public interface IUserService
{
    Task<User> CreateAsync(User userToCreate);
    Task<User> Login(LoginRequest loginRequest);
    Task<User> GetByIdAsync(Guid userId);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid userId);
    Task<List<Clock>> GetClocksByUser(Guid id);
    Task<Clock> AddClock(CreateClockDto clock, Guid userId);
    Task<ToDo> AddTodo(ToDo toDo, Guid userId);
}