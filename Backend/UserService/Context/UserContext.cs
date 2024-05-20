using Microsoft.EntityFrameworkCore;
using UserService.Model;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace UserService.Context;

public class UserContext : DbContext
{
    public UserContext(){}
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=User;Username=postgres;Password=postgres");
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Clock> Clocks { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ToDo> Todos { get; set; }


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
            });
        modelBuilder.Entity<ToDo>().HasData(
            new ToDo
            {
                Id = Guid.Parse("f656d97d-63b7-451a-91ee-0e620e652c9e"),
                UserId = Guid.Parse("5f3bb5af-e982-4a8b-8590-b620597a7360"),
            });
        base.OnModelCreating(modelBuilder);
    }
}