using Application.DTOs;
using Application.DTOs.Group;
using Application.DTOs.Post;
using Application.Requests;
using Application.Requests.Group;
using Application.Requests.Post;
using Domain.Entities;
using IdentityModel;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Api.Endpoints;

public static class RouteExtension
{
    public static void MapEnpoints(this WebApplication app)
    {
        app.UsePostEndpoints();
        app.UseGroupEndpoints();
    }

    private static void UsePostEndpoints(this WebApplication app)
    {
        app.MapGet("api/post", GetPosts)
            .AllowAnonymous();
        app.MapPost("api/post", CreatePost)
            .ProducesProblem(400)
            .RequireAuthorization();
        app.MapDelete("api/post", DeletePost)
            .RequireAuthorization();
        app.MapPut("api/post", UpdatePost)
            .RequireAuthorization();
    }
    
    public static void UseGroupEndpoints(this WebApplication app)
    {
        app.MapGet("api/group/{Id:guid}", GetGroup);
        app.MapPost("api/group", CreateGroup)
            .RequireAuthorization();
        app.MapPut("api/group", UpdateGroup)
            .RequireAuthorization();
        app.MapPost("api/group/leave", LeaveFromGroup)
            .RequireAuthorization();
    }

    private static async Task<IResult> LeaveFromGroup(HttpContext context,
        [FromForm] IdGroupRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<LeaveFromGroupRequest>();
        req.CustomerId = Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value);
        
        await mediator.Send(req);
        return Results.Ok();
    }

    private static async Task<IResult> UpdateGroup(HttpContext context,
    [FromForm] UpdateGroupRequestDto dto,
    [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<UpdateGroupRequest>();
        req.CustomerId = Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value);
        await mediator.Send(req);
        return Results.Ok();
    }

    private static async Task<IResult> CreateGroup(HttpContext context,
        [FromBody] CreateGroupRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<CreateGroupRequest>();
        req.Userid = new CustomerId(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value);
        await mediator.Send(req);
        return Results.Ok();
    }
    
    private static async Task<IResult> GetGroup(HttpContext context,
        [AsParameters] GetGroupRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<GetGroupRequest>();
        var res = await mediator.Send(req);
        return Results.Json(res);
    }
    
    private static async Task<IResult> GetPosts(HttpContext context,
        [AsParameters] GetPostRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<GetPostRequest>();
        var res = await mediator.Send(req);
        return Results.Json(res);
    }

    private static async Task<IResult> CreatePost(HttpContext context,
        [FromBody] CreatePostRequestDTO dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<CreatePostRequest>();
        req.Userid = new CustomerId(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value);
        var res = await mediator.Send(req);
        return Results.Json(res);
    }
    
    private static async Task<IResult> DeletePost(HttpContext context,
        [FromBody] DeletePostRequestDTO dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<DeletePostRequest>();
        req.Userid = new CustomerId(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value);
        var res = await mediator.Send(req);
        return Results.Json(res);
    }

    private static async Task<IResult> UpdatePost(HttpContext context,
        [FromBody] UpdatePostRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<UpdatePostRequest>();
        req.Userid = new CustomerId(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value);
        await mediator.Send(req);
        return Results.NoContent();
    }
}