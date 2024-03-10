namespace Domain.Entities;

public class SubComment : Comment
{
    public SubComment(string content) : base(content)
    {
    }
    public SubComment() { }
    
    public Guid ParentComment { get; set; }
}