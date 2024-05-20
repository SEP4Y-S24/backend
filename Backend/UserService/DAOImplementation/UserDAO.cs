using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UserService.Context;
using UserService.IDAO;
using UserService.Model;

namespace UserService.DAOImplementation;

public class UserDAO: IUserDAO
{
    private readonly UserContext context;

    public UserDAO(UserContext dbContext)
    {
        context = dbContext;
    }
    public virtual async ValueTask<User?> GetByAsync(Expression<Func<User, bool>> filter)
    {
        return await context.Users.Where(filter).FirstOrDefaultAsync();
    }
    public Task<User?> GetByIdAsync(Guid userId)
    {
        if (userId==Guid.Empty)
        {
            throw new ArgumentNullException("The given user Id is null");
        }

        User? existing = context.Users.Include(u=>u.MessagesSent).Include(u=>u.MessagesRecieved).Include(u=>u.Clocks).Include(u=>u.Todos).FirstOrDefault(t => t.Id == userId);
        return Task.FromResult(existing);
    }

    public async Task<User> CreateAsync(User user)
    {
        user.Clocks = new List<Clock>();
        user.Todos = new List<ToDo>();
        user.MessagesRecieved = new List<Message>();
        user.MessagesSent = new List<Message>();
        user.Id = Guid.NewGuid();
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
        if (userId==Guid.Empty)
        {
            throw new ArgumentNullException("User ID is null");
        }
        
        User? toDelete = await GetByIdAsync(userId);
        context.Users.Remove(toDelete);
        await context.SaveChangesAsync();
    }

    public async Task DeleteClock(Guid clockId, Guid userId)
    {
        User? user = await GetByIdAsync(userId);
        Clock? clock = user.Clocks.FirstOrDefault(c => c.Id.Equals(clockId));
        if (clock != null)
        {
            user.Clocks.Remove(clock);
        }
    }
}