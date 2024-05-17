using System.ComponentModel.DataAnnotations;

namespace AlarmServices.Model;

public class Alarm
{
    [Key]
    public Guid Id { get; set;}
    public Guid ClockId { get; set; }
    public DateTime SetOffTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsSnoozed { get; set; }

    public Alarm(Guid clockId, DateTime setOffTime, bool isActive)
    {
        Id = Guid.NewGuid();
        ClockId = clockId;
        SetOffTime = setOffTime;
        IsActive = isActive;
        IsSnoozed = false;
    }

    public Alarm()
    {
        
    }
}