using BuildingBlocks;
using Domain;
using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class CommentDbContext: DbContext
{
    public CommentDbContext(DbContextOptions<CommentDbContext> options) : base(options)
    {
    }
    
    public DbSet<Comment> Comment { get; set; }
    public DbSet<SubComment> SubComment { get; set; }

    public DbSet<OutBoxMessage> OutBoxMessage { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
       
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(InfrastructureRoot).Assembly);
    }
}