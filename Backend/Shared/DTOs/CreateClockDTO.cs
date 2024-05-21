namespace Shared.Dtos;

public class CreateClockDTO
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public long TimeOffset { get; set; }

}