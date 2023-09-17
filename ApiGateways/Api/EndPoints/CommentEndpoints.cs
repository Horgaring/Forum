using Application.DTOs;
using GrpcClientcomment;
using Infrastructure;
using Infrastructure.Clients;
using Microsoft.AspNetCore.Mvc;
using GetCommentRequest = Application.DTOs.GetCommentRequest;

namespace Api.EndPoints;

public static class CommentEndpoints
{
    public static void MapEnpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/comments", CreateComment);
        app.MapDelete("api/comments", DeleteComment);
        app.MapGet("api/comments", GetComment);
        app.MapPut("api/comments", UpdateComment);
    }

    private async static Task<IResult> UpdateComment(HttpContext context
        ,[FromBody]CommentRequest request
        ,[FromServices] CommentGrpcService service)
    {
       var resp = await service.UpdateCommentAsync(new CommentRequestDTO()
        {
            Content = request.Content,
            Id = request.Id
        });
       if (resp == false)
       {
           return Results.BadRequest();
       }
       return Results.NoContent();
    }

    private async static Task<IResult> GetComment(HttpContext context
        ,[FromBody]GetCommentRequest request
        ,[FromServices] CommentGrpcService service)
    {
        var resp = await service.GetCommentAsync(new GrpcClientcomment.GetCommentRequest()
        {
            Listnum = request.Listnum,
            Listsize = request.Listzize,
            Postid = request.Postid
        });
        return Results.Ok(resp);
    }

    private async static Task<IResult> DeleteComment(HttpContext context
        ,[FromBody]DeleteCommentRequest request
        ,[FromServices] CommentGrpcService service)
    {
        var resp = await service.RemoveCommentAsync(new DeleteCommentRequestDTO()
        {
            Id = request.Id
        });
        if (resp == false)
        {
            return Results.BadRequest();
        }
        return Results.NoContent();
    }

    private async static Task<IResult> CreateComment(HttpContext context
        ,[FromBody]CommentRequest request
        ,[FromServices] CommentGrpcService service)
    {
        var resp = await service.CreateCommentAsync(new CommentRequestDTO()
        {
            Content = request.Content,
            Id = request.Id
        });
        return Results.CreatedAtRoute<CommentResponseDTO>(value: resp);
    }
}