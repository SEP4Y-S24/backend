﻿using System.ComponentModel.DataAnnotations;

namespace EfcDatabase.Model;

public class Message
{
    [Key]
    public Guid Id { get; set;}
    public DateTime DateOfCreation { get; set; }

    public string Body { get; set; }
    public User Sender { get; set; }
    public Guid SenderId { get; set; }
    public Clock Clock { get; set; }
    public Guid ClockId { get; set; }
    public User Reciever { get; set; }
    public Guid ReceiverId { get; set; }

    public Message()
    {
        
    }
}