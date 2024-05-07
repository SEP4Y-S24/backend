using Application.DAO;
using EfcDatabase.Context;
using EfcDatabase.Model;

namespace Services.Services;

public class UserDAO: IUserDAO
{
    private readonly UserContext context;

    public UserDAO(UserContext dbContext)
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