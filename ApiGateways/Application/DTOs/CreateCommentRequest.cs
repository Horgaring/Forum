namespace Application.DTOs;

public class CreateCommentRequest
{
    public int Id { get; set; }
    public Guid Postid { get; set; }
    public string Content { get; set; }
}