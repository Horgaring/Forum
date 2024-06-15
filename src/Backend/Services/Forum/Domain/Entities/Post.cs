using BuildingBlocks;
using BuildingBlocks.Core.Model;

namespace Domain.Entities;

public class Post : AggregateRoot<Guid>
{
    public CustomerId User { get; set; }
    
    public string Title { get; set; }

    public byte[]? Content { get; set; }
    
    public string? Description { get; set; }
    
    public Group Group { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastUpdate { get; set; }

    public void Update(string title, string? description, byte[]? content)
    {
        LastUpdate = DateTime.UtcNow;
        Title = title;
        Description = description;
        Content = content;
    }
}

