using System.Linq.Expressions;
using AAlarmService.IDAO;
using AAlarmService.Context;
using AAlarmService.Model;
using Microsoft.EntityFrameworkCore;

namespace AAlarmService.DAOImplementation;

public class AlarmDAO: IAlarmDAO
{
    private readonly AlarmContext _alarmContext;

    public AlarmDAO(AlarmContext dbContext)
    {
        _alarmContext = dbContext;
    }

    public async Task<Alarm> CreateAsync(Alarm alarm)
    {
        try
        {
            DateTime.SpecifyKind(alarm.SetOffTime, DateTimeKind.Utc);            if (alarm == null && alarm.ClockId == null)
            {
                throw new ArgumentNullException($"The given Alarm object {alarm} is null");
            }

            await _alarmContext.Alarms.AddAsync(alarm);
            await _alarmContext.SaveChangesAsync();
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
        return await _alarmContext.Set<Alarm>().Where(filter).ToListAsync();
    }

    public async Task<IEnumerable<Alarm>> GetAllAsync()
    {
        return await _alarmContext.Set<Alarm>().ToListAsync();
    }

    public async Task UpdateAsync(Alarm alarm)
    {
        Alarm? dbEntity = await GetByIdAsync(alarm.Id);

        if (alarm==null)
        {
            throw new ArgumentNullException("Alarm object is not found in the database");
        }
        _alarmContext.Alarms.Entry(dbEntity).CurrentValues.SetValues(alarm);

        _alarmContext.Update(dbEntity);
        await _alarmContext.SaveChangesAsync();
    }

    public async Task<Alarm?> GetByIdAsync(Guid alarmId)
    {
        if (alarmId.Equals(null))
        {
            throw new ArgumentNullException("The given Alarm Id is null");
        }
        
        Alarm? existing = await _alarmContext.Alarms.FirstOrDefaultAsync(alarm => alarm.Id == alarmId);
        return existing;
    }

    public async Task DeleteAsync(Guid alarmId)
    {
        if (alarmId==null)
        {
            throw new ArgumentNullException("Alarm Id is null");
        }

        Alarm? toDelete = await GetByIdAsync(alarmId);
        _alarmContext.Alarms.Remove(toDelete);
        await _alarmContext.SaveChangesAsync();
    }
}