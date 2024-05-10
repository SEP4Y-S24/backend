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

    public async Task UpdateAsync(User user)  
    {
        User? dbEntity = await GetByIdAsync(user.Id);

        if (user==null)
        {
            throw new ArgumentNullException();
        }
        context.Entry(dbEntity).CurrentValues.SetValues(user);

        context.Update(dbEntity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId)
    {
        if (userId==null)
        {
            throw new ArgumentNullException("User ID is null");
        }
        
        User? toDelete = await GetByIdAsync(userId);
        context.Users.Remove(toDelete);
        await context.SaveChangesAsync();
    }
}