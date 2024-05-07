using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore;

namespace EfcDatabase.Context;

public class ClockContext : DbContext
{
/*    public ClockContext(DbContextOptions<ClockContext> options) : base(options)
   
    {
    }
*/
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=pgsql-container;Port=5432;Database=sep4;Username=postgres;Password=postgres");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Clock> Clocks { get; set; }
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
                TimeZone = 'G',
            });
        base.OnModelCreating(modelBuilder);
    }
}