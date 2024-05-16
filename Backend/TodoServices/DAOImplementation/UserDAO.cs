using ClockServices.Context;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoServices.IDAO;
using TodoServices.Model;

namespace TodoServices.DAOImplementation;

public class UserDAO: IUserDao
{
    private readonly ToDoContext context;

    public UserDAO(ToDoContext dbContext)
    {
        context = dbContext;
    }

    public Task<User?> GetByIdAsync(Guid userId)
    {
        if (userId==null)
        {
            throw new ArgumentNullException("The given user Id is null");
        }

        User? existing = context.Users.FirstOrDefault(t => t.Id == userId);
        return Task.FromResult(existing);
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