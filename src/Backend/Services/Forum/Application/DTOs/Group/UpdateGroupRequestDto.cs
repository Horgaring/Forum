using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs.Group;

public class UpdateGroupRequestDto
{
    [FromForm]
    public string Name { get; set; }
    [FromForm]
    public IFormFile Avatar { get; set; }
}