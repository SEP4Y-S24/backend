namespace Shared.DTOs;

public class CreateTagDto
{
    public string Name { get; set; }
    public string Colour { get; set; }
    public Guid UserId { get; set; }
}