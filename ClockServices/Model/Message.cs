using System.ComponentModel.DataAnnotations;

namespace ClockServices.Model;

public class Message
{
    [Key]
    public Guid Id { get; set;}

    public string Body { get; set; }
    public Clock? Clock { get; set; }
    public Guid ClockId { get; set; }
    
    public Message()
    {
        
    }
}