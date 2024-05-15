using AlarmServices.Model;

namespace AlarmServices.DTOs;

public class AlarmsDTO
{
    public IEnumerable<Alarm> Alarms { get; set; }
}