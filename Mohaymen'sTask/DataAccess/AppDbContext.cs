using Microsoft.EntityFrameworkCore;
using Mohaymen_sTask.Entities;

namespace Mohaymen_sTask.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasData(new List<User>()
            {
                new User() { Id = 1, UserName = "Admin", Password = "12345" , IsAvailable = true},
                new User() { Id = 2, UserName = "User", Password = "12345" , IsAvailable = true},
                new User() { Id = 3, UserName = "Mahsa", Password = "6789" , IsAvailable = true},
                new User() { Id = 4, UserName = "Mahya", Password = "98765" , IsAvailable = false}
            });

        base.OnModelCreating(modelBuilder);
    }
}