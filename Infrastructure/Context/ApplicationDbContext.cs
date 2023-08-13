using Domain;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;
namespace Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<Forum> Forum { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }
}