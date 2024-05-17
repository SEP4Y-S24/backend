using AlarmServices.Model;

namespace AAlarmServices.DTOs;

public class AlarmsDTO
{
    public IEnumerable<AlarmDTO> Alarms { get; set; }
}