using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Post
{
    public Guid Id { get; set; }
    public string Userid { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }

    public void Update(string title, string description)
    {
        Title = Title;
        Description = description;
    }
}

