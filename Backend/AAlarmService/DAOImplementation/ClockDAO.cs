using System.Linq.Expressions;
using AAlarmServices.IDAO;
using AlarmServices.Context;
using AlarmServices.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AAlarmServices.DAOImplementation;

public class ClockDAO: IClockDAO
{
    private readonly AlarmContext _alarmContext;

    public ClockDAO(AlarmContext dbContext)
    {
        this._alarmContext = dbContext;
    }

    public async Task<Clock> CreateAsync(Clock clock)
    {
        EntityEntry<Clock> added = await _alarmContext.Clocks.AddAsync(clock);
        await _alarmContext.SaveChangesAsync();
        return added.Entity;
    }

    public async Task<IEnumerable<Clock>> GetAll()
    {
        return await _alarmContext.Set<Clock>().Include(e => e.Messages).ToListAsync();

    }

    public async Task<IEnumerable<Clock>> GetAllByAsync(Expression<Func<Clock, bool>> filter)
    {
        return await _alarmContext.Set<Clock>().Include(e => e.Messages).Where(filter).ToListAsync();

    }

    public async Task<Clock> UpdateAsync(Clock clockToUpdate)
    {
        Clock? existing =  _alarmContext.Clocks.FirstOrDefault(post => post.Id == clockToUpdate.Id);
        if (existing == null)
        {
            throw new Exception($"Clock with id {clockToUpdate.Id} does not exist!");
        }

        _alarmContext.Entry(existing).CurrentValues.SetValues(clockToUpdate);
        if (clockToUpdate.Messages != null)
        {
            existing.Messages = new List<Message>();
            foreach (var message in clockToUpdate.Messages)
            {
                clockToUpdate.Messages.Add(message);
            }
        }

        _alarmContext.Clocks.Update(existing);

        _alarmContext.SaveChanges();

        return clockToUpdate;
    }

    public Task<Clock?> GetByIdAsync(Guid clockId)
    {
        if (clockId.Equals(null))
        {
            throw new ArgumentNullException("Clock's id is null!");
        }
        Clock? existing = _alarmContext.Clocks.FirstOrDefault(t => t.Id == clockId);
        return Task.FromResult(existing);
    }
    public Task<long> GetOffsetByIdAsync(Guid clockId)
    {
        if (clockId.Equals(null))
        {
            throw new ArgumentNullException("Clock's id is null!");
        }
        Clock? existing = _alarmContext.Clocks.FirstOrDefault(t => t.Id == clockId);
        return Task.FromResult(existing.TimeOffset); ;
    }
    public async Task DeleteAsync(Guid id)

    {
        var entity = await GetByIdAsync(id);

        if (entity == null)
        {
            throw new ArgumentException("Clock is null!");
        }

        _alarmContext.Remove(entity);

        await _alarmContext.SaveChangesAsync();

    }
}