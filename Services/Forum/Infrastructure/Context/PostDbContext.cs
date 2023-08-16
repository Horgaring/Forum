using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class PostDbContext: DbContext
{
    public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
    {
    }
    public DbSet<Post>? Post { get; set; }
    public DbSet<Comment>? Comment { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
    }
}