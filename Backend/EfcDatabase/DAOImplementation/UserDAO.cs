using EfcDatabase.IDAO;
using EfcDatabase.Context;
using EfcDatabase.Model;

namespace EfcDatabase.DAOImplementation;

public class UserDAO: IUserDAO
{
    private readonly ClockContext context;

    public UserDAO(ClockContext dbContext)
    {
        context = dbContext;
    }

    public Task<User?> GetByIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Clock> CreateAsync(Clock clock)
    {
        throw new NotImplementedException();
    }
}