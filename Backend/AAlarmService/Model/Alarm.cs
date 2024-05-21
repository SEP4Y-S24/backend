using System.ComponentModel.DataAnnotations;

namespace AAlarmService.Model;

public class  Alarm
{
    [Key]
    public Guid Id { get; set;}
    public string Name { get; set; }
    public Guid ClockId { get; set; }
    public DateTime SetOffTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsSnoozed { get; set; }

    public Alarm(Guid clockId,string name, DateTime setOffTime, bool isActive)
    {
        Id = Guid.NewGuid();
        Name = name;
        ClockId = clockId;
        SetOffTime = setOffTime;
        IsActive = isActive;
        IsSnoozed = false;
    }

    public Alarm()
    {
        
    }
}