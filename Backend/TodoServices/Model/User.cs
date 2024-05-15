using System.ComponentModel.DataAnnotations;
using TodoServices.Model;

namespace TodoServices.Model;

public class User
{
    [Key]
    public Guid Id { get; set;}
    public ICollection<Todo?> Todos { get; set; }
}