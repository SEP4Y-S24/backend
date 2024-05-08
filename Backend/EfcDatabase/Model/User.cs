using System.ComponentModel.DataAnnotations;

namespace EfcDatabase.Model;

public class User
{
    [Key]
    public Guid Id { get; set;}
    public virtual ICollection<Message> Messages { get; set; }
}