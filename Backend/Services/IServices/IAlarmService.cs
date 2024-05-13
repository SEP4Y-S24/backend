using EfcDatabase.Model;

namespace Services.IServices;

public interface IAlarmService
{
    Task<Alarm> CreateAsync(Alarm alarmToCreate);
    Task<Alarm?> GetByIdAsync(Guid alarmId);
    Task UpdateAsync(Alarm alarm);
    Task DeleteAsync(Guid alarmId);
    Task EnableAlarm(Guid alarmId);
    Task DisableAlarm(Guid alarmId);
}