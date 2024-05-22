using System.ComponentModel.DataAnnotations;

namespace Models;

public class  Alarm
{
    [Key]
    public Guid Id { get; set;}
    public string Name { get; set; }
    public Guid ClockId { get; set; }
    public TimeOnly SetOffTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsSnoozed { get; set; }

    public Alarm(Guid clockId,string name, TimeOnly setOffTime, bool isActive)
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