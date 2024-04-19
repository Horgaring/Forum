using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Infrastructure.Context;

public class GroupConfigurations: IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder
            .HasMany(p => p.Followers)
            .WithMany(p => p.Groups);

        builder.HasIndex(p => p.Name).IsUnique();
        
        builder
            .HasMany(p => p.Posts)
            .WithOne(p => p.Group)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(p => p.Owner)
            .WithMany(p => p.OwnGroups)
            .OnDelete(DeleteBehavior.Cascade);
        
        
    }
}