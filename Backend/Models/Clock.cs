using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Models;

public class Clock
{
    [Key]
    public Guid Id { get; set;}
    public string Name { get; set;}
    public long TimeOffset { get; set; }
    public User Owner { get; set; }
    public Guid OwnerId { get; set; }
    public virtual ICollection<Message> Messages { get; set; }

  /*  public Alarm Alarm { get; set; }
    public Guid AlarmId { get; set; }*/
   
    public Clock(User owner, string name, long timeOffset)
    {
        OwnerId = owner.Id;
        Owner = owner;
        Name = name;
        TimeOffset = timeOffset;
    }
    public Clock()
    {

    }
}