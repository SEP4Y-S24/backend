using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoServices.Model;

namespace TodoServices.Model;

public class Todo
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
    public Status Status { get; set; }
    
    [ForeignKey("user_Id")]
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<Tag> Tags { get; set; }

    public Todo(User user, string name, string? description, DateTime deadline, Status status)
    {
        User = user;
        UserId = user.Id;
        Name = name;
        Description = description;
        Deadline = deadline;
        Status = status;
    }

    public Todo()
    {
        
    }
}