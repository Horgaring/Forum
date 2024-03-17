using BuildingBlocks.Core.Model;

namespace Domain.Entities;

public class Post : Entity<Guid>
{
    public CustomerId User { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public Group Group { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastUpdate { get; set; }

    public void Update(string title, string? description)
    {
        LastUpdate = DateTime.Now;
        Title = Title;
        Description = description;
    }
}

