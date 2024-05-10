using EfcDatabase.IDAO;
using EfcDatabase.Context;
using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDatabase.DAOImplementation;

public class UserDAO: IUserDAO
{
    private readonly ClockContext context;

    public UserDAO(ClockContext dbContext)
    {
        context = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        if (userId==null)
        {
            throw new ArgumentNullException("The given user Id is null");
        }

        User? user = await context.Users.FindAsync(userId);
        return user;
    }

    public async Task<User> CreateAsync(User user)
    {
        EntityEntry<User> added = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task UpdateAsync(Guid userId)  //not full logic
    {
        User? toUpdate = await GetByIdAsync(userId);
        
        User? existing = context.Users.FirstOrDefault(user => user.Id == userId);
        if (existing==null)
        {
            throw new Exception($"User with id {userId} does not exist.");
        }

        context.Users.Update(toUpdate);
        await context.SaveChangesAsync();
    }
}