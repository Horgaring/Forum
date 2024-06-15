using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs.Post;

public class CreatePostRequestDTO
{
    [FromForm]
    public Guid GroupId { get; set; }
    [FromForm]
    public string Title { get; set; }
    [FromForm]
    public string? Description { get; set; }
    [FromForm]
    public IFormFile? Image { get; set; }
}