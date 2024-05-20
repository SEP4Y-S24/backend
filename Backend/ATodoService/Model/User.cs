using System.ComponentModel.DataAnnotations;

namespace ATodoService.Model;

public class User
{
    [Key]
    public Guid Id { get; set;}
    public ICollection<Todo?> Todos { get; set; }
}