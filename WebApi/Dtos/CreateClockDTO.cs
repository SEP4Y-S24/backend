namespace WebApplication1.Dtos;

public class CreateClockDTO
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public long TimeOffset { get; set; }

}