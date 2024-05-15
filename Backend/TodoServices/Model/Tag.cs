using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoServices.Model;

public class Tag
{
    public Guid Id { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Index(IsUnique=true)]
    public string name { get; set; }
    public ICollection<Todo> Todos { get; set; }
}