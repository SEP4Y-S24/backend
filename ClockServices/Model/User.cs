using System.ComponentModel.DataAnnotations;

namespace ClockServices.Model;

public class User
{
    [Key]
    public Guid Id { get; set;}
    public ICollection<Clock?> Clocks { get; set; }

}