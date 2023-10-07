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
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}