namespace Application.DTOs;

public class GetCommentRequest
{
    public int ListNum { get; set; }
    public int ListSize { get; set; }
    public string Postid { get; set; }
}