using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EfcDatabase.Model;

public class Alarm
{
    [Key]
    public Guid Id { get; set;}
    public Clock Clock { get; set; }
    public Guid ClockId { get; set; }
    public DateTime SetOffTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsSnoozed { get; set; }

    public Alarm(Clock clock, DateTime setOffTime, bool isActive)
    {
        Id = Guid.NewGuid();
        Clock = clock;
        ClockId = clock.Id;
        SetOffTime = setOffTime;
        IsActive = isActive;
        IsSnoozed = false;
    }

    public Alarm()
    {
        
    }
}