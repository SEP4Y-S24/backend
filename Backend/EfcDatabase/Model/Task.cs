using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EfcDatabase.Model;

public class Task
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public Status Status { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Task(User user, string name, string description, DateTime deadline, Status status)
    {
        User = user;
        UserId = user.Id;
        Name = name;
        Description = description;
        Deadline = deadline;
        Status = status;
    }
    

}