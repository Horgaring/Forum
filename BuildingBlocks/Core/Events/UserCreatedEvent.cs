namespace BuildingBlocks.Core.Events;

public class UserCreatedEvent: DomainEvent
{
    public UserCreatedEvent(string name, Guid id)
    {
        Name = name;
        Id = id;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}