using System.Data;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class PostDbContext: DbContext
{
    public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
    {
    }
    public DbSet<Post> Post { get; set; }
    
    public DbSet<Group> Groups { get; set; }
    
    public DbSet<CustomerId> CustomersId { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>()
            .HasMany(p => p.Followers)
            .WithMany(p => p.Groups);

        
        
        modelBuilder.Entity<Group>()
            .HasMany(p => p.Posts)
            .WithOne(p => p.Group);
        
        modelBuilder.Entity<Group>()
            .HasOne(p => p.Owner)
            .WithMany(p => p.OwnGroups);
    }
    
}