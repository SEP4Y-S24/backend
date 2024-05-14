
using System.ComponentModel.DataAnnotations;

namespace AlarmServices.Model;

public class User
{
    [Key]
    public Guid Id { get; set;}
    public virtual ICollection<Message> MessagesRecieved { get; set; }
    public virtual ICollection<Message> MessagesSent { get; set; }
    public ICollection<Clock?> Clocks { get; set; }

}