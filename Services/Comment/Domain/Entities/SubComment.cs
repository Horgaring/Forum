namespace Domain.Entities;

public class SubComment : Comment
{
    public SubComment(string content) : base(content)
    {
    }
    
    public int ParentComment { get; set; }
}