using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace UserService.Model;

public class ToDo
{
    [Key]
    public Guid Id { get; set; } 
    [ForeignKey("user_Id")]
    public Guid UserId { get; set; }
    public User User { get; set; }

    public ToDo(User user, string name)
    {
        User = user;
        UserId = user.Id;
    }

    public ToDo()
    {
        
    }
    

}