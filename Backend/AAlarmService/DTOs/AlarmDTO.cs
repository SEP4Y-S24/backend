namespace AAlarmServices.DTOs;

public class AlarmDTO
{
    public Guid Id { get; set;}
    public Guid ClockId { get; set; }
    public DateTime SetOffTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsSnoozed { get; set; }
}