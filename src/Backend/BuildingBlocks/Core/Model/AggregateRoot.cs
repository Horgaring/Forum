using BuildingBlocks.Core.Model;

namespace BuildingBlocks;

public abstract class AggregateRoot<T> : Entity<T>
{
    private List<IDomainEvent> _events = new();

    public void RaiseEvent(IDomainEvent @event) =>
        _events.Add(@event);

    public void ClearEvents() => _events.Clear();

    public IList<IDomainEvent> GetDomainEvents() => _events.ToList();
   
}
