namespace UserService.Dtos;

public class CreateClockDto
{
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }

}