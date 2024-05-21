
using System.ComponentModel.DataAnnotations;

namespace AAlarmService.Model;

public class User
{
    [Key]
    public Guid Id { get; set;}

}