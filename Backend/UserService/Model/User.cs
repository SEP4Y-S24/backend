using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserService.Model;

public class User
{
    [Key]
    public Guid Id { get; set;}
    [System.ComponentModel.DataAnnotations.Schema.Index(IsUnique=true)]
    public string Email { get; set; }
    public string Name { get; set; }
    public int AvatarId { get; set; }
    public string PasswordHash { get; set; }
    public virtual ICollection<Message> MessagesRecieved { get; set; }
    public virtual ICollection<Message> MessagesSent { get; set; }
    public ICollection<Clock?> Clocks { get; set; }
    public ICollection<ToDo?> Todos { get; set; }

}