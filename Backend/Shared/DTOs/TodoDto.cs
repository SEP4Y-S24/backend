using System.ComponentModel.DataAnnotations.Schema;
using Models;

namespace Shared.dtos;

public class TodoDto
{
    //public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
    public Status Status { get; set; }
    public Guid UserId { get; set; }
    public List<TagDto> Tags { get; set; }

}