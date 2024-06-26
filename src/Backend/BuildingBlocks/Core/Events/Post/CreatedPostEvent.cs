using BuildingBlocks.Core.Model;

namespace BuildingBlocks.Core.Events.Post;

public class CreatedPostEvent : IDomainEvent
{
    public Guid Id { get; set; }
    
    public Guid Userid { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastUpdate { get; set; }
}