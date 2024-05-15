using AlarmServices.Context;
using AlarmServices.IDAO;
using AlarmServices.Model;
using Microsoft.EntityFrameworkCore;

namespace AlarmServices.DAOImplementation;

public class AlarmDAO: IAlarmDAO
{
    private readonly ClockContext _clockContext;

    public AlarmDAO(ClockContext dbContext)
    {
        _clockContext = dbContext;
    }

    public async Task<Alarm> CreateAsync(Alarm alarm)
    {
        try
        {
            if (alarm == null && alarm.ClockId == null)
            {
                throw new ArgumentNullException($"The given Alarm object {alarm} is null");
            }

            await _clockContext.Alarms.AddAsync(alarm);
            await _clockContext.SaveChangesAsync();
            return alarm;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Alarm>> GetAllAsync()
    {
        return await _clockContext.Set<Alarm>().ToListAsync();
    }

    public async Task UpdateAsync(Alarm alarm)
    {
        Alarm? dbEntity = await GetByIdAsync(alarm.Id);

        if (alarm==null)
        {
            throw new ArgumentNullException();
        }
        _clockContext.Entry(dbEntity).CurrentValues.SetValues(alarm);

        _clockContext.Update(dbEntity);
        await _clockContext.SaveChangesAsync();
    }

    public async Task<Alarm?> GetByIdAsync(Guid alarmId)
    {
        if (alarmId.Equals(null))
        {
            throw new ArgumentNullException("The given Alarm Id is null");
        }
        
        Alarm? existing = await _clockContext.Alarms.FirstOrDefaultAsync(alarm => alarm.Id == alarmId);
        return existing;
    }

    public async Task DeleteAsync(Guid alarmId)
    {
        if (alarmId==null)
        {
            throw new ArgumentNullException("Alarm Id is null");
        }

        Alarm? toDelete = await GetByIdAsync(alarmId);
        _clockContext.Alarms.Remove(toDelete);
        await _clockContext.SaveChangesAsync();
    }
}