using System.Linq.Expressions;
using AAlarmService.Model;

namespace AAlarmService.IDAO;

public interface IAlarmDAO
{
    Task<Alarm> CreateAsync(Alarm alarm);
    Task UpdateAsync(Alarm alarm);
    Task<Alarm?> GetByIdAsync(Guid alarmId);
    Task DeleteAsync(Guid alarmId);
    Task<IEnumerable<Alarm>> GetAllAsync();
    Task<IEnumerable<Alarm>> GetAllByAsync(Expression<Func<Alarm, bool>> filter);

}