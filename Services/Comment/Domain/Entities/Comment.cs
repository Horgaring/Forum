namespace Domain.Entities;

public class Comment
{
    public Comment(string content)
    {
        Content = content;
    }

    public int Id { get; set; }
    public Guid Postid { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public void Update(string content)
    {
        Content = content;
    }

}