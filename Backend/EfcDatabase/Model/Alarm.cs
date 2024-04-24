using System.ComponentModel.DataAnnotations;

namespace EfcDatabase.Model;

public class Alarm
{
    [Key]
    public Guid Id { get; set;}
}