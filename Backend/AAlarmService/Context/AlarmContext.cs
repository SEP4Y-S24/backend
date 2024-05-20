using AlarmServices.Model;
using Microsoft.EntityFrameworkCore;

namespace AlarmServices.Context;

public class AlarmContext: DbContext
{
    public AlarmContext() { }
    public AlarmContext(DbContextOptions<AlarmContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=clockcontext.postgres.database.azure.com;" +
            "Port=5432;" +
            "Database=AlarmService;" +
            "Username=clockcontext;" +
            "Password=postgres123!;");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Clock> Clocks { get; set; }
    
    public DbSet<Alarm> Alarms { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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