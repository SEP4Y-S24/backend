﻿

namespace Models;

public class Tag
{
    public Guid Id { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Index(IsUnique=true)]
    public string Name { get; set; }
    public string Colour { get; set; }
    public Guid UserId { get; set; }
    public User Owner { get; set; }
    public ICollection<Todo> Todos { get; set; }
    public ICollection<Event> Events { get; set; }
}