

namespace ATodoService.Model;

public class Tag
{
    public Guid Id { get; set; }
    // [System.ComponentModel.DataAnnotations.Schema.Index(IsUnique=true)]
    //TODO: Add unique constraint
    public string name { get; set; }
    public ICollection<Todo> Todos { get; set; }
}