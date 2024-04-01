namespace BuildingBlocks.Core.Events.Post;

public class DeletedPostEvent : DomainEvent
{
    public Guid PostId { get; set; }
}