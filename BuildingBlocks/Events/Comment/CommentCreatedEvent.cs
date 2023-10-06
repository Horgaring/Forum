namespace BuildingBlocks.Events.Comment;

public class CommentCreatedEvent
{
    public string Content { get; set; }
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public DateTime Date { get; set; }
    public Guid PostId { get; set; }
}