using Models;
using Shared.Dtos;

namespace Shared.IService;

public interface IUserService
{
    Task<User> CreateAsync(User userToCreate);
    Task<User> Login(LoginRequest loginRequest);
    Task<User?> GetByIdAsync(Guid userId);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid userId);
    Task<List<Clock>> GetClocksByUser(Guid id);
  //  Task<Clock> AddClock(CreateClockDTO clock, Guid userId);
   // Task<Todo> AddTodo(Todo toDo, Guid userId);
}