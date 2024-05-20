using AlarmServices.Model;

namespace AAlarmServices.DTOs;

public class AlarmsDTO
{
    public ICollection<AlarmDTO> Alarms { get; set; }
}