using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs.Post;

public class GetPostsRequestDto ([FromQuery]int pagesize,[FromQuery]int pagenum)
{

    
    public int PageSize { get; set; } = pagesize;
    public int PageNum { get; set; } = pagenum;
}