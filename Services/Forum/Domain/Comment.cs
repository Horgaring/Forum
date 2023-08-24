namespace Domain;

public class Comment
{
    public int Id { get; set; }
    public Guid Userid { get; set; }
    public string? Content { get; set; }
    public DateTime Date { get; set; }
}