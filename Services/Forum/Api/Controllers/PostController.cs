using System.IdentityModel.Tokens.Jwt;
using Application.DTOs.Post;
using Application.PostRequests;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers;

[ApiController]
[Route("api/")]
public class PostController : Controller
{
    private readonly IMediator _mediator;
    
    public PostController(IMediator mediator)=>
        _mediator = mediator;
    
    [HttpGet("forums")]
    public async Task<JsonResult> GetPosts([FromQuery]string query
        ,[FromQuery]int pagesize
        ,[FromQuery]int pagenum)
    {
        var request = new GetPostRequest()
        {
            Query = query,
            PageNum = pagenum,
            PageSize = pagesize
        };
        var res = await _mediator.Send(request);
        return Json(res);
    }
    [HttpPost("forums")]
    public async Task<IActionResult> CreatePost([FromForm] PostRequestDTO post)
    {
        var request = new CreatePostRequest()
        {
            Userid = HttpContext.User.Claims
                .First(op => op.Type ==  JwtClaimTypes.Subject).Value,
            Title = post.Title,
            Description = post.Description,
            Date = DateTime.UtcNow
        };
        await _mediator.Send(request);
        return Ok();
    }
    [HttpPut("forums")]
    public async Task<IActionResult> UpdatePost([FromForm] PostRequestDTO post)
    {
        var request = new UpdatePostRequest()
        {
            Userid = HttpContext.User.Claims
                .First(op => op.Type ==  JwtClaimTypes.Subject).Value,
            Title = post.Title,
            Description = post.Description,
            Date = DateTime.UtcNow
        };
        await _mediator.Send(request);
        return NoContent();
    }
    
}
