using System.Linq.Expressions;
using UserService.Model;

namespace UserService.IDAO;

public interface IUserDAO
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid userId);
    Task DeleteClock(Guid clockId, Guid userId);
    ValueTask<User?> GetByAsync(Expression<Func<User, bool>> filter);

}