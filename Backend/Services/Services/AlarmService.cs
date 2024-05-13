using EfcDatabase.IDAO;
using EfcDatabase.Model;
using Services.IServices;

namespace Services.Services;

public class AlarmService:IAlarmService
{
    private readonly IAlarmDAO _alarmDao;

    public AlarmService(IAlarmDAO alarmDao)
    {
        _alarmDao = alarmDao;
    }


    public async Task<Alarm> CreateAsync(Alarm alarmToCreate)
    {
        try
        {
            Alarm alarm = await _alarmDao.CreateAsync(alarmToCreate);
            return alarm;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Alarm?> GetByIdAsync(Guid alarmId)
    {
        try
        {
            Alarm? alarm = await _alarmDao.GetByIdAsync(alarmId);
            return alarm;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new InvalidOperationException();
        }
    }

    public async Task UpdateAsync(Alarm alarm)
    {
        try
        {
            await _alarmDao.UpdateAsync(alarm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task DeleteAsync(Guid alarmId)
    {
        try
        {
            await _alarmDao.DeleteAsync(alarmId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task EnableAlarm(Guid alarmId)
    {
        try
        {
            Alarm? alarm = await _alarmDao.GetByIdAsync(alarmId);
            alarm.IsActive = true;
            await _alarmDao.UpdateAsync(alarm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DisableAlarm(Guid alarmId)
    {
        try
        {
            Alarm? alarm = await _alarmDao.GetByIdAsync(alarmId);
            alarm.IsActive = false;
            alarm.IsSnoozed = false;
            await _alarmDao.UpdateAsync(alarm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Alarm>> GetAllAsync()
    {
        try
        {
            return await _alarmDao.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}