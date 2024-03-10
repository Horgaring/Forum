namespace Application.DTOs.Post;

public class CreatePostRequestDTO
{
    public Guid GroupId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}