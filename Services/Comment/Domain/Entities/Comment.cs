using BuildingBlocks.Core.Model;

namespace Domain.Entities;

public class Comment : Entity<Guid>
{
    public Comment(string content)
    {
        Content = content;
    }
    public Guid Postid { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public void Update(string content)
    {
        Content = content;
    }

}