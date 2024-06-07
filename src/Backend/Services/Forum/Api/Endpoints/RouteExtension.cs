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
            .AllowAnonymous()
            .RequireAuthorization();
        app.MapGet("api/posts/{id:guid}", GetPostById)
            .AllowAnonymous()
            .RequireAuthorization();
        app.MapGet("api/posts", GetPosts)
            .AllowAnonymous();
        app.MapPost("api/posts", CreatePost)
            .ProducesProblem(400)
            .RequireAuthorization();
        app.MapDelete("api/posts", DeletePost)
            .RequireAuthorization();
        app.MapPut("api/posts", UpdatePost)
            .RequireAuthorization();
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
        app.MapPost("api/groups/leave/{dto:guid}", LeaveFromGroup)
            .RequireAuthorization();
        app.MapPost("api/groups/join/{dto:guid}", JoinInGroup)
            .RequireAuthorization();
    }

    private static async Task<IResult> JoinInGroup(HttpContext context,
    [FromRoute] Guid dto,
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
        [FromRoute] Guid dto,
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
        var req = id.Adapt<GetPostRequest>();
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