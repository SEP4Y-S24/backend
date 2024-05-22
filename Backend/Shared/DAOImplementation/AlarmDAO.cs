using System.Linq.Expressions;
using Shared.IDAO;
using Shared.Context;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Shared.DAOImplementation;

public class AlarmDAO: IAlarmDAO
{
    private readonly ClockContext _context;

    public AlarmDAO(ClockContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Alarm> CreateAsync(Alarm alarm)
    {
        try
        {
            DateTime.SpecifyKind(alarm.SetOffTime, DateTimeKind.Utc);            if (alarm == null && alarm.ClockId == null)
            {
                throw new ArgumentNullException($"The given Alarm object {alarm} is null");
            }

            await _context.Alarms.AddAsync(alarm);
            await _context.SaveChangesAsync();
            return alarm;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<IEnumerable<Alarm>> GetAllByAsync(Expression<Func<Alarm, bool>> filter)
    {
        return await _context.Set<Alarm>().Where(filter).ToListAsync();
    }

    public async Task<IEnumerable<Alarm>> GetAllAsync()
    {
        return await _context.Set<Alarm>().ToListAsync();
    }

    public async Task UpdateAsync(Alarm alarm)
    {
       if (alarm==null)
       {
            throw new ArgumentNullException("Alarm object is not found in the database");
       }
       Alarm? dbEntity = await GetByIdAsync(alarm.Id);
       if (dbEntity == null)
       {
           throw new ArgumentNullException("Alarm object is not found in the database");
       }
 
       _context.Alarms.Entry(dbEntity).CurrentValues.SetValues(alarm);

       _context.Update(dbEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Alarm?> GetByIdAsync(Guid alarmId)
    {
        if (alarmId.Equals(null))
        {
            throw new ArgumentNullException("The given Alarm Id is null");
        }
        
        Alarm? existing = await _context.Alarms.FirstOrDefaultAsync(alarm => alarm.Id == alarmId);
        return existing;
    }

    public async Task DeleteAsync(Guid alarmId)
    {
        if (alarmId.Equals(null))
        {
            throw new ArgumentNullException("Alarm Id is null");
        }

        Alarm? toDelete = await GetByIdAsync(alarmId);
        if(toDelete == null)
        {
            throw new ArgumentNullException("Alarm object is not found in the database");
        }
        _context.Alarms.Remove(toDelete);
        await _context.SaveChangesAsync();
    }
}