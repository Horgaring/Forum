namespace Application.DTOs;

public class CreateSubCommentRequest
{
    public int Id { get; set; }
    public Guid Postid { get; set; }
    public string Content { get; set; }
    public int ParentComment { get; set; }
}