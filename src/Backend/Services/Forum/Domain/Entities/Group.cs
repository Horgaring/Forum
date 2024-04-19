using BuildingBlocks;
using BuildingBlocks.Core.Model;

namespace Domain.Entities;

public class Group : AggregateRoot<Guid>
{
    public string Name { get; set; }
    
    public byte[] AvatarPath { get; set; }
    
    public CustomerId Owner { get; set; }
    
    public List<Post> Posts { get; set; }
    
    public List<CustomerId> Followers { get; set; }
}
