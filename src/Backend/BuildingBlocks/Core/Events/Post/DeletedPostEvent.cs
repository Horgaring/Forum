using BuildingBlocks.Core.Model;

namespace BuildingBlocks.Core.Events.Post;

public class DeletedPostEvent : IDomainEvent
{
    public Guid PostId { get; set; }
}