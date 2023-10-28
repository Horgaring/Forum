using System.Net;
using Application.DTOs;
using GrpcClientpost;
using Infrastructure.Clients;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Api.EndPoints;

public static class PostEnpoints
{
    public static void MapEnpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/posts", CreatePost);
        app.MapDelete("api/posts", DeletePost);
        app.MapGet("api/posts", GetPost);
        app.MapPut("api/posts", UpdatePost);
    }

    private async static Task<IResult> UpdatePost(HttpContext context
        ,[FromBody]PostRequest request
        ,[FromServices] PostGrpcService service)
    {
        var resp = await service.UpdatePostAsync(new PostRequestGrpc()
        {
            Description = request.Description,
            Title = request.Title
        });
        if (resp == false)
        {
            return Results.BadRequest();
        }
        return Results.NoContent();
    }

    private async static Task<IResult> GetPost(HttpContext context
        ,[FromBody]GetPostRequest request
        ,[FromServices] PostGrpcService service)
    {
        var resp = await service.GetPostAsync(request.Adapt<GetPostRequestGrpc>());
        return Results.Json<List<PostResponseGrpc>>(resp,statusCode:200);
    }

    private async static Task<IResult> DeletePost(HttpContext context
        ,[FromBody]DeletePostRequest request
        ,[FromServices] PostGrpcService service)
    {
        var resp = await service.RemovePostAsync(new DeletePostRequestGrpc()
        {
            Id = request.Id
        });
        if (resp == false)
        {
            return Results.BadRequest();
        }
        return Results.NoContent();
    }

    private async static Task<IResult> CreatePost(HttpContext context
        ,[FromBody]CreatePostRequest request
        ,[FromServices] PostGrpcService service)
    {
        var resp = await service.CreatePostAsync(request.Adapt<PostRequestGrpc>());
        return Results.Json<PostResponseGrpc>(resp,statusCode:201);
    }
}