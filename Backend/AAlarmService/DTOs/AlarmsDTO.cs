using AAlarmService.Model;

namespace AAlarmService.DTOs;

public class AlarmsDTO
{
    public ICollection<AlarmDTO> Alarms { get; set; }
}