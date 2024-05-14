using AlarmServices.Model;
using Microsoft.EntityFrameworkCore;

namespace AlarmServices.Context;

public class ClockContext: DbContext
{
    public ClockContext() { }
    public ClockContext(DbContextOptions<ClockContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=alarmService;Username=postgres;Password=331425");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Clock> Clocks { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Alarm> Alarms { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
                Id = Guid.Parse("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                ClockId = Guid.Parse("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                IsSnoozed = false,
                SetOffTime = DateTime.UtcNow.AddHours(1), // SetOffTime (example: 1 hour from now)
                IsActive = true // IsActive
            });
        base.OnModelCreating(modelBuilder);
    }
}