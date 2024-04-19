namespace Application.DTOs.Post;

public class UpdatePostRequestDto
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}