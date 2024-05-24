using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Event
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Status Status { get; set; }
    
    [ForeignKey("user_Id")]
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<Tag> Categories { get; set; }

}