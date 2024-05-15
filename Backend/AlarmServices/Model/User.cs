
using System.ComponentModel.DataAnnotations;

namespace AlarmServices.Model;

public class User
{
    [Key]
    public Guid Id { get; set;}

}