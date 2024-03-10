using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<DayActivity> DayActivities { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>()
            .HasMany(p => p.Activitys)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);
        builder.Entity<User>().HasIndex(p => p.Email).IsUnique();
        builder.Entity<User>().HasIndex(p => p.UserName).IsUnique();
    }
}