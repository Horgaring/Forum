using Application.DTOs;
using GrpcClientcomment;
using Infrastructure;
using Infrastructure.Clients;
using Mapster;
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
        ,[FromBody]UpdateCommentRequest request
        ,[FromServices] CommentGrpcService service)
    {
       var resp = await service.UpdateCommentAsync(request.Adapt<UpdateCommentRequestDTO>());
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
        var resp = await service.GetCommentAsync(request.Adapt<GrpcClientcomment.GetCommentRequest>());
        return Results.Ok(resp);
    }

    private async static Task<IResult> DeleteComment(HttpContext context
        ,[FromBody]DeleteCommentRequest request
        ,[FromServices] CommentGrpcService service)
    {
        var resp = await service.RemoveCommentAsync(request.Adapt<DeleteCommentRequestDTO>());
        if (resp == false)
        {
            return Results.BadRequest();
        }
        return Results.NoContent();
    }

    private async static Task<IResult> CreateComment(HttpContext context
        ,[FromBody]CreateCommentRequest request
        ,[FromServices] CommentGrpcService service)
    {
        var resp = await service.CreateCommentAsync(request.Adapt<CreateCommentRequestDTO>());
        return Results.CreatedAtRoute<CommentResponseDTO>(value: resp);
    }
    private async static Task<IResult> CreateSubComment(HttpContext context
        ,[FromBody]CreateSubCommentRequest request
        ,[FromServices] CommentGrpcService service)
    {
        var resp = await service.CreateSubCommentAsync(request.Adapt<CreateSubCommentRequestDTO>());
        return Results.CreatedAtRoute<CommentResponseDTO>(value: resp);
    }
}