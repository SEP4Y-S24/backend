

namespace Models;

public class Category
{
    public Guid Id { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Index(IsUnique=true)]
    public string Name { get; set; }
    public ICollection<Todo> Todos { get; set; }
}