using AlarmServices.Model;

namespace AlarmServices.IDAO;

public interface IAlarmDAO
{
    Task<Alarm> CreateAsync(Alarm alarm);
    Task UpdateAsync(Alarm alarm);
    Task<Alarm?> GetByIdAsync(Guid alarmId);
    Task DeleteAsync(Guid alarmId);
    Task<IEnumerable<Alarm>> GetAllAsync();
}