using AlarmServices.Model;

namespace AlarmServices.IService;

public interface IAlarmService
{
    Task<Alarm> CreateAsync(Alarm alarmToCreate);
    Task<Alarm?> GetByIdAsync(Guid alarmId);
    Task UpdateAsync(Alarm alarm);
    Task DeleteAsync(Guid alarmId);
    Task EnableAlarmAsync(Guid alarmId);
    Task DisableAlarmAsync(Guid alarmId);
    Task<IEnumerable<Alarm>> GetAllAsync();
}