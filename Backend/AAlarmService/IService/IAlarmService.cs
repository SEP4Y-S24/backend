using AlarmServices.Model;

namespace AAlarmServices.IService;

public interface IAlarmService
{
    Task<Alarm> CreateAsync(Alarm alarmToCreate);
    Task<Alarm?> GetByIdAsync(Guid alarmId);
    Task UpdateAsync(Alarm alarm);
    Task DeleteAsync(Guid alarmId);
    Task EnableAlarmAsync(Guid alarmId);
    Task DisableAlarmAsync(Guid alarmId);
    Task<IEnumerable<Alarm>> GetAllAsync();
    Task<IEnumerable<Alarm>> GetAllByClockAsync(Guid clockId);
    Task<Alarm> ToggleAlarmAsync(Guid alarmId, bool? state);
}