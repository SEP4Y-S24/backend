
using Models;

namespace Shared.dtos;

public class CreateTodo
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
    public Status Status { get; set; }
    public Guid UserId { get; set; }
}