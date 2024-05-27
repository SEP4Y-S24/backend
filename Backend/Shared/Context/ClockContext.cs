using Models;
using Microsoft.EntityFrameworkCore;

namespace Shared.Context;

public class ClockContext: DbContext
{
    public ClockContext() { }
    public ClockContext(DbContextOptions<ClockContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=clockcontext.postgres.database.azure.com;" +
            "Port=5432;" +
            "Database=Sep4MicroServices;" +
            "Username=clockcontext;" +
            "Password=postgres123!;");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Clock> Clocks { get; set; }
    
    public DbSet<Alarm> Alarms { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Todo> Todos { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Measurement> Measurements { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>()
            .HasOne(m => m.User1)
            .WithMany(u => u.Addressee)
            .HasForeignKey(m => m.User1id);
        modelBuilder.Entity<Contact>()
            .HasOne(m => m.User2)
            .WithMany(u => u.Requester)
            .HasForeignKey(m => m.User2id);
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Reciever)
            .WithMany(u => u.MessagesRecieved)
            .HasForeignKey(m => m.ReceiverId);
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.MessagesSent)
            .HasForeignKey(m => m.SenderId);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("5f3bb5af-e982-4a8b-8590-b620597a7360"),
                    Name = "Test User",
                    Email = "email@gmail.com",
                    AvatarId = 1,
                    PasswordHash = "psgzhj,sxzjh",
                });
        modelBuilder.Entity<Clock>().HasData(
            new Clock
            {
                Id = Guid.Parse("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                OwnerId = Guid.Parse("5f3bb5af-e982-4a8b-8590-b620597a7360"),
                Name = "Test Clock",
                TimeOffset = 0,
            });
        modelBuilder.Entity<Alarm>().HasData(
            new Alarm
            {
                Id = Guid.Parse("ac96066e-c7da-4b53-9203-d1bf4b5a88b9"),
                ClockId = Guid.Parse("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                Name = "Default alarm",
                IsSnoozed = false,
                SetOffTime = new TimeOnly(1,20), // SetOffTime (example: 1 hour from now)
                IsActive = true // IsActive
            });
        modelBuilder.Entity<Todo>().HasData(
            new Todo
            {
                Id = Guid.Parse("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                UserId = Guid.Parse("5f3bb5af-e982-4a8b-8590-b620597a7360"),
                Deadline = DateTime.UtcNow.AddDays(7),
                Name = "Hello",
                Description = "hello description",
                Status = Status.InProgress
            });
        
        base.OnModelCreating(modelBuilder);
    }
}