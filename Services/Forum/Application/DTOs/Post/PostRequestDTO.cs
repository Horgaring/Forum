namespace Application.DTOs.Post;

public class PostRequestDTO
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}