﻿using System.ComponentModel.DataAnnotations;

namespace EfcDatabase.Model;

public class User
{
    [Key]
    public Guid Id { get; set;}
    public virtual ICollection<Message> MessagesRecieved { get; set; }
    public virtual ICollection<Message> MessagesSent { get; set; }
    public ICollection<Clock?> Clocks { get; set; }
    public ICollection<ToDo?> Todos { get; set; }

}