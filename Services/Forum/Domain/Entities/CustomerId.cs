using BuildingBlocks;
using BuildingBlocks.Core.Model;

namespace Domain.Entities;

public class CustomerId : AggregateRoot<Guid>
{
    public CustomerId(Guid id, string name)
    {
        Name = name;
        Id = id;
    }
    public CustomerId()
    {
    }
    
    public string Name { get; set; }
    public IEnumerable<Group>? Groups { get; set; }
    public IEnumerable<Group>? OwnGroups { get; set; }
    
    public List<Post> Posts { get; set; }
}