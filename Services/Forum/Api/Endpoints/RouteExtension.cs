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
using Serilog.Core;

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
        app.MapGet("api/group/", GetGroup);
        app.MapPost("api/group", CreateGroup)
            .RequireAuthorization()
            .DisableAntiforgery();
        app.MapPut("api/group", UpdateGroup)
            .RequireAuthorization()
            .DisableAntiforgery();
        app.MapPost("api/group/leave", LeaveFromGroup)
            .RequireAuthorization();
        app.MapPost("api/group/join", JoinInGroup)
            .RequireAuthorization();
    }

    private static async Task<IResult> JoinInGroup(HttpContext context,
    [FromBody] IdGroupRequestDto dto,
    [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<JoinInGroupReqiest>();
        req.User = new CustomerId(Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value),
            context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Name).Value);
        
        
        await mediator.Send(req);
        return Results.Ok();
    }

    private static async Task<IResult> LeaveFromGroup(HttpContext context,
        [FromBody] IdGroupRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<LeaveFromGroupRequest>();
        req.CustomerId = Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value);
        
        await mediator.Send(req);
        return Results.Ok();
    }

    private static async Task<IResult> UpdateGroup(HttpContext context,
    [AsParameters] UpdateGroupRequestDto dto,
    [FromServices] IMediator mediator)
    {
        
        var req = new UpdateGroupRequest()
        {
            Name = dto.Name
        };
        req.Avatar = dto.Avatar;
        req.CustomerId = Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value);
        
        await mediator.Send(req);
        return Results.Ok();
    }

    private static async Task<IResult> CreateGroup(HttpContext context,
        [AsParameters] CreateGroupRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = new CreateGroupRequest()
        {
            Name = dto.Name
        };
        req.Avatar = dto.Avatar;
        req.User = new CustomerId(Guid.Parse( context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value),
            context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Name).Value);
        
        
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
        req.User = new CustomerId(Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value),
            context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Name).Value);
        var res = await mediator.Send(req);
        return Results.Json(res);
    }
    
    private static async Task<IResult> DeletePost(HttpContext context,
        [FromBody] DeletePostRequestDTO dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<DeletePostRequest>();
        req.User = new CustomerId(Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value),
            context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Name).Value);
        var res = await mediator.Send(req);
        return Results.Json(res);
    }

    private static async Task<IResult> UpdatePost(HttpContext context,
        [FromBody] UpdatePostRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<UpdatePostRequest>();
        req.User = new CustomerId(Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value), 
            context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Name).Value);
        await mediator.Send(req);
        return Results.NoContent();
    }
}