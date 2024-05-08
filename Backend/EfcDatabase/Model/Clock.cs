using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace EfcDatabase.Model;

public class Clock
{
    [Key]
    public Guid Id { get; set;}
    public string Name { get; set;}
    public char TimeZone { get; set; }

    public User Owner { get; set; }
    public Guid OwnerId { get; set; }
    public virtual ICollection<Message> Messages { get; set; }

   /* public Alarm alarm { get; set; }
    public Guid alarmId { get; set; }
   */
    public Clock(User owner, String name, char timeZone)
    {
        OwnerId = owner.Id;
        Owner = owner;
        Name = name;
        TimeZone = timeZone;
        //alarm = new Alarm();
    }
    public Clock()
    {
        /*alarm = new Alarm();
        alarmId = alarm.Id;*/
    }
}