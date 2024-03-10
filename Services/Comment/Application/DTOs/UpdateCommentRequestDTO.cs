namespace Application.DTOs;

public class UpdateCommentRequestDTO
{
    public Guid id { get; set; }
    public string Content { get; init; }
}