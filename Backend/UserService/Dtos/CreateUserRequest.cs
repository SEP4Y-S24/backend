namespace UserService.Dtos;

public class CreateUserRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
    public int AvatarId { get; set; }
    public string Password { get; set; }

}