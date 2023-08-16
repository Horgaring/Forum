namespace Application.DTOs.Post;

public class PostResponseDTO
{
    
    public Guid Userid { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    
}