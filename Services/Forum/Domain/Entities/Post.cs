using BuildingBlocks.Core.Model;

namespace Domain.Entities;

public class Post : Entity<Guid>
{
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

