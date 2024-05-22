using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.IDAO;
using Shared.Context;
using Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Shared.DAOImplementation;

public class UserDAO: IUserDAO
{
    private readonly ClockContext _context;

    public UserDAO(ClockContext dbContext)
    {
        _context = dbContext;
    }

    public virtual async ValueTask<User?> GetByAsync(Expression<Func<User, bool>> filter)
    {
        return await _context.Users.Where(filter).FirstOrDefaultAsync();
    }
    public Task<User?> GetByIdAsync(Guid userId)
    {
        if (userId==Guid.Empty)
        {
            throw new ArgumentNullException("The given user Id is null");
        }

        User? existing = _context.Users.Include(u=>u.MessagesSent).Include(u=>u.MessagesRecieved).Include(u=>u.Clocks).Include(u=>u.Todos).FirstOrDefault(t => t.Id == userId);
        return Task.FromResult(existing);
    }

    public async Task<User> CreateAsync(User user)
    {
        user.Clocks = new List<Clock>();
        user.Todos = new List<Todo>();
        user.MessagesRecieved = new List<Message>();
        user.MessagesSent = new List<Message>();
        user.Id = Guid.NewGuid();
        EntityEntry<User> added = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task UpdateAsync(User user)  
    {
        User? dbEntity = await GetByIdAsync(user.Id);

        if (dbEntity==null)
        {
            throw new ArgumentNullException();
        }
        _context.Entry(dbEntity).CurrentValues.SetValues(user);

        _context.Update(dbEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId)
    {
        if (userId==Guid.Empty)
        {
            throw new ArgumentNullException("User ID is null");
        }
        
        User? toDelete = await GetByIdAsync(userId);
        if (toDelete == null)
        {
            throw new ArgumentNullException("User does not exist");
        }
        _context.Users.Remove(toDelete);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteClock(Guid clockId, Guid userId)
    {
        User? user = await GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentNullException("User does not exist");
        }
        Clock? clock = user.Clocks.FirstOrDefault(c => c.Id.Equals(clockId));
        if (clock != null)
        {
            user.Clocks.Remove(clock);
        }
        await _context.SaveChangesAsync();
    }

}