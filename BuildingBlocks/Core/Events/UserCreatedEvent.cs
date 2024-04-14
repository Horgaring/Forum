using BuildingBlocks.Core.Model;

namespace BuildingBlocks.Core.Events;

public class UserCreatedEvent: IDomainEvent
{
    public UserCreatedEvent(string name, Guid id)
    {
        Name = name;
        Id = id;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}