namespace AAlarmServices.DTOs;

public class CreateAlarmDTO
{
    public Guid ClockId { get; set; }
    public DateTime SetOffTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsSnoozed { get; set; }
}