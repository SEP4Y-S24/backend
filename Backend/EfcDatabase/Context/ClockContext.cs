using System.Data.Entity;
using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

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

    public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Clock> Clocks { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<ToDo> Todos { get; set; }

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
        modelBuilder.Entity<ToDo>().HasData(
            new ToDo
            {
                User = new User(),
                Id = Guid.Parse("f656n17d-63b7-451a-91ee-0e620e652c9e"),
                Deadline = DateTime.UtcNow.AddDays(7),
                Name = "Hello",
                Description = "hello description",
                Status = Status.InProgress
            });
        base.OnModelCreating(modelBuilder);
    }
}