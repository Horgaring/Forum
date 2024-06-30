using Application;
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
        app.MapGet("api/posts/groups/{groupid:guid}", GetPostsByGroupId)
            .RequireAuthorization();
        app.MapGet("api/posts/{id:guid}", GetPostById)
            .RequireAuthorization();
        app.MapGet("api/posts", GetPosts)
            .AllowAnonymous();
        app.MapPost("api/posts", CreatePost)
            .ProducesProblem(400)
            .RequireAuthorization()
            .DisableAntiforgery();
        app.MapDelete("api/posts", DeletePost)
            .RequireAuthorization();
        app.MapPut("api/posts", UpdatePost)
            .RequireAuthorization()
            .DisableAntiforgery();
    }
    
    public static void UseGroupEndpoints(this WebApplication app)
    {
        app.MapGet("api/groups", GetGroups);
        app.MapPost("api/groups", CreateGroup)
            .RequireAuthorization()
            .DisableAntiforgery();
        app.MapGet("api/groups/{Id:guid}", GetGroup)
            .RequireAuthorization()
            .DisableAntiforgery();
        app.MapPut("api/groups", UpdateGroup)
            .RequireAuthorization()
            .DisableAntiforgery();
        app.MapPost("api/groups/leave/{dto}", LeaveFromGroup)
            .RequireAuthorization();
        app.MapPost("api/groups/join/{dto}", JoinInGroup)
            .RequireAuthorization();
    }

    private static async Task<IResult> JoinInGroup(HttpContext context,
    [FromRoute] string dto,
    [FromServices] IMediator mediator)
    {
        var req = new JoinInGroupReqiest(){Name = dto};
        req.User = new CustomerId(Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value),
            context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Name).Value);
        
        
        await mediator.Send(req);
        return Results.Ok();
    }

    private static async Task<IResult> LeaveFromGroup(HttpContext context,
        [FromRoute] string dto,
        [FromServices] IMediator mediator)
    {
        var req = new LeaveFromGroupRequest(){Name = dto};
        req.CustomerId = Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value);
        
        await mediator.Send(req);
        return Results.Ok();
    }

    private static async Task<IResult> UpdateGroup(HttpContext context,
    [AsParameters] UpdateGroupRequestDto dto,
    [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<UpdateGroupRequest>();
        if (dto.Image != null)
        {
            using var fs = new MemoryStream();
            dto.Image.CopyTo(fs);
            req.Avatar = fs.ToArray();
        }
        req.CustomerId = Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value);
        
        await mediator.Send(req);
        return Results.Ok();
    }

    private static async Task<IResult> CreateGroup(HttpContext context,
        [AsParameters] CreateGroupRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<CreateGroupRequest>();
        if (dto.Image != null)
        {
            using var fs = new MemoryStream();
            dto.Image.CopyTo(fs);
            req.Avatar = fs.ToArray();
        }
        req.User = new CustomerId(Guid.Parse( context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value),
            context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Name).Value);
        
        
        await mediator.Send(req);
        return Results.Ok();
    }
    
    private static async Task<IResult> GetGroups(HttpContext context,
        [AsParameters] GetGroupsRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<GetGroupsRequest>();
        var res = await mediator.Send(req);
        return Results.Json(res);
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
        [AsParameters] GetPostsRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<GetPostsRequest>();
        var res = await mediator.Send(req);
        return Results.Json(res);
    }

    private static async Task<IResult> GetPostsByGroupId(HttpContext context,
        [AsParameters] GetPostsByGroupIdRequestDto request,
        [FromServices] IMediator mediator)
    {
        var req = request.Adapt<GetPostsByGroupIdRequest>();
        var res = await mediator.Send(req);
        return Results.Json(res);
    }

    private static async Task<IResult> GetPostById(HttpContext context,
        [FromRoute] Guid id,
        [FromServices] IMediator mediator)
    {
        var req = new GetPostRequest(){Id = id};
        var res = await mediator.Send(req);
        return Results.Json(res);
    }

    private static async Task<IResult> CreatePost(HttpContext context,
        [AsParameters] CreatePostRequestDTO dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<CreatePostRequest>();
        if (dto.Image != null)
        {
            using var fs = new MemoryStream();
            dto.Image.CopyTo(fs);
            req.Content = fs.ToArray();
        }
        req.User = new CustomerId(Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value),
            context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Name).Value);
        var res = await mediator.Send(req);
        return Results.Json(res);
    }
    
    private static async Task<IResult> DeletePost(HttpContext context,
        [AsParameters] DeletePostRequestDTO dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<DeletePostRequest>();
        req.User = new CustomerId(Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value),
            context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Name).Value);
        await mediator.Send(req);
        return Results.Ok();
    }

    private static async Task<IResult> UpdatePost(HttpContext context,
        [AsParameters] UpdatePostRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var req = dto.Adapt<UpdatePostRequest>();
        if (dto.Image != null)
        {
            using var fs = new MemoryStream();
            dto.Image.CopyTo(fs);
            req.Content = fs.ToArray();
        }
        req.User = new CustomerId(Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value), 
            context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Name).Value);
        await mediator.Send(req);
        return Results.NoContent();
    }
}