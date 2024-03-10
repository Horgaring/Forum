using BuildingBlocks.Core.Model;

namespace Domain.Entities;

public class CustomerId : Entity<Guid>
{
    public CustomerId(string id)
    {
        Id = Guid.Parse(id);
    }
    public CustomerId(){}
    
    public IEnumerable<Group>? Groups { get; set; }
    public IEnumerable<Group>? OwnGroups { get; set; }
}