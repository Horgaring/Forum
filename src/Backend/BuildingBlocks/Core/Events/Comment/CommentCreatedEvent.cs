using BuildingBlocks.Core.Model;

namespace BuildingBlocks.Core.Events.Comment;

public class CommentCreatedEvent : IDomainEvent
{
    public string Content { get; set; }
    public Guid CustomerId { get; set; }
    
    public string CustomerName { get; set; }
    public DateTime Date { get; set; }
    public Guid PostId { get; set; }
}