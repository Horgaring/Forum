namespace Application.DTOs;

public class GetPostRequest
{
    public int Pagenum { get; set; }
    public int Pagesize { get; set; }
    public string Query { get; set; }
}