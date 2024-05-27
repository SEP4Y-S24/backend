using Models;

namespace Shared.DTOs;

public class EventDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public Status Status { get; set; }
    public Guid UserId { get; set; }

}