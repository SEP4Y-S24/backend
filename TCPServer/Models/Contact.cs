using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Contact
{
    public Guid id { get; set; }
    public Guid User1id { get; set; }
    public Guid User2id { get; set; }
    public string Email1 { get; set; }
    public string Email2 { get; set; }
    public User User1 { get; set; }
    public User User2 { get; set; }
}