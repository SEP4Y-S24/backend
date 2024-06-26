﻿using Models;
using Shared.IDAO;
using Shared.IService;

namespace Shared.Service;

public class AlarmService: IAlarmService
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

    public async Task EnableAlarmAsync(Guid alarmId)
    {
        try
        {
            Alarm? alarm = await _alarmDao.GetByIdAsync(alarmId);
            if (alarm == null)
                throw new Exception($"Alarm with ID {alarmId} not found!");
            alarm.IsActive = true;
            await _alarmDao.UpdateAsync(alarm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DisableAlarmAsync(Guid alarmId)
    {
        try
        {
            Alarm? alarm = await _alarmDao.GetByIdAsync(alarmId);
            if (alarm == null)
                throw new Exception($"Alarm with ID {alarmId} not found!");
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

    public async Task<IEnumerable<Alarm>> GetAllByClockAsync(Guid clockId)
    {
        try
        {
            return await _alarmDao.GetAllByAsync(a=>a.ClockId.Equals(clockId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }    }

    public async Task<Alarm> ToggleAlarmAsync(Guid alarmId, bool? state)
    {
        Alarm? existing = await _alarmDao.GetByIdAsync(alarmId);

        if (existing == null)
        {
            throw new Exception($"Alarm with ID {alarmId} not found!");
        }
        if(state.Equals(null))
        {
            throw new Exception("The given state is not valid");
        }
        existing.IsActive = state.Value;
        await _alarmDao.UpdateAsync(existing);
        return existing;
    }
}