using System.ComponentModel.DataAnnotations;

namespace EfcDatabase.Model;

public class Timer
{
    [Key]
    public Guid Id { get; set;}
}