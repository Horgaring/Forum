using BuildingBlocks.Core.Model;

namespace BuildingBlocks;

public abstract class AggregateRoot<T> : Entity<T>
{
    private List<DomainEvent> _events = new();

    public void RaiseEvent(DomainEvent @event) =>
        _events.Add(@event);

    public void ClearEvents() => _events.Clear();

    public IList<DomainEvent> GetDomainEvents() => _events.ToList();
   
}
