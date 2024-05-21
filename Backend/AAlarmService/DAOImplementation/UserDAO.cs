using AAlarmService.IDAO;
using AAlarmService.Context;
using AAlarmService.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AAlarmService.DAOImplementation;

public class UserDAO: IUserDAO
{
    private readonly AlarmContext _alarmContext;

    public UserDAO(AlarmContext dbContext)
    {
        _alarmContext = dbContext;
    }

    public Task<User?> GetByIdAsync(Guid userId)
    {
        if (userId==null)
        {
            throw new ArgumentNullException("The given user Id is null");
        }

        User? existing = _alarmContext.Users.FirstOrDefault(t => t.Id == userId);
        return Task.FromResult(existing);
    }

    public async Task<User> CreateAsync(User user)
    {
        EntityEntry<User> added = await _alarmContext.Users.AddAsync(user);
        await _alarmContext.SaveChangesAsync();
        return added.Entity;
    }

    public async Task UpdateAsync(User user)  
    {
        User? dbEntity = await GetByIdAsync(user.Id);

        if (user==null)
        {
            throw new ArgumentNullException();
        }
        _alarmContext.Entry(dbEntity).CurrentValues.SetValues(user);

        _alarmContext.Update(dbEntity);
        await _alarmContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId)
    {
        if (userId==null)
        {
            throw new ArgumentNullException("User ID is null");
        }
        
        User? toDelete = await GetByIdAsync(userId);
        _alarmContext.Users.Remove(toDelete);
        await _alarmContext.SaveChangesAsync();
    }

}