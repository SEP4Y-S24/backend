using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore;

namespace EfcDatabase.Context;

public class ClockContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Clock> Clocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Clock;Username=postgres;Password=331425");
    }
}