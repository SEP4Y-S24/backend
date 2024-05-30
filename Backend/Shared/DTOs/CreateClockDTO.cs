namespace Shared.Dtos;

public class CreateClockDTO
{
    
    public Guid ClockId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public long TimeOffset { get; set; }

}