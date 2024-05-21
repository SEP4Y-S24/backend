namespace AClockService.Dtos;

public class ClockDTO
{
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public long TimeOffset { get; set; }

}