using System.Net;
using Application.DTOs;
using Application.Requests;
using Domain.ValueObjects;
using IdentityModel;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Api.Endpoints;

public static class RouteExtension
{
    public static void MapEnpoints(this WebApplication app)
    {
        app.MapGroup("api/comment")
            .RequireAuthorization()
            .UseCommentEndpoints();
    }
    public static void UseCommentEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("api/comment", GetComments)
            .WithDescription("Get comments")
            .ProducesProblem(400)
            .Produces((int)HttpStatusCode.OK)
            .AllowAnonymous();
        app.MapGet("api/subcomment", GetSubComments)
            .WithDescription("Get subcomments")
            .ProducesProblem(400)
            .Produces((int)HttpStatusCode.OK)
            .AllowAnonymous();
        app.MapPost("api/comment", CreateComment)
            .ProducesProblem(401)
            .ProducesProblem(400)
            .Produces((int)HttpStatusCode.Created);
        app.MapPost("api/subcomment", CreateSubComment)
            .Produces((int)HttpStatusCode.Created)
            .ProducesProblem(401)
            .ProducesProblem(400);;
        app.MapDelete("api/comment", DeleteComment)
            .ProducesProblem(401)
            .ProducesProblem(400)
            .Produces((int)HttpStatusCode.OK);;
        app.MapPut("api/comment", UpdateComment)
            .ProducesProblem(401)
            .ProducesProblem(400)
            .Produces((int)HttpStatusCode.OK);
    }

    private static async Task<IResult> GetComments(HttpContext context,
        [AsParameters] GetCommentRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var request = dto.Adapt<GetCommentRequest>();
        var resp = await mediator.Send(request);
        return Results.Json(resp);
    }

    private static async Task<IResult> GetSubComments(HttpContext context,
        [AsParameters] GetCommentRequestDto dto,
        [FromServices] IMediator mediator)
    {
        var request = dto.Adapt<GetSubCommentRequest>();
        var resp = await mediator.Send(request);
        return Results.Json(resp);
    }
    
    private static async Task<IResult> CreateComment(HttpContext context,
        [FromBody] CreateCommentRequestDTO dto,
        [FromServices] IMediator mediator)
    {
        var request = dto.Adapt<CreateCommentRequest>();
        Log.Information("{@Claim}", Guid.Parse(context.User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value));
        request.CustomerInfo = new CustomerInfo()
        {
            Id = Guid.Parse(context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Subject).Value)
        };
        Log.Information("{@Claim}", request.CustomerInfo.Id);
        var resp = await mediator.Send(request);
        return Results.Json(resp,statusCode: (int)HttpStatusCode.Created);
    }

    private static async Task<IResult> CreateSubComment(HttpContext context,
        [FromBody] CreateSubCommentRequestDTO dto,
        [FromServices] IMediator mediator)
    {
        var request = dto.Adapt<CreateSubCommentRequest>();
        request.CustomerInfo = new CustomerInfo()
        {
            Id = Guid.Parse(context.User.Claims
                .First(op => op.Type == JwtClaimTypes.Subject).Value)
        };
        var resp = await mediator.Send(request);
        return Results.Json(resp,statusCode:(int)HttpStatusCode.Created);
    }

    private static async Task<IResult> DeleteComment(HttpContext context,
        [FromBody] DeleteCommentRequestDTO dto,
        [FromServices] IMediator mediator)
    {
        var request = dto.Adapt<DeleteCommentRequest>();
        await mediator.Send(request);
        return Results.Ok();
    }

    private static async Task<IResult> UpdateComment(HttpContext context,
        [FromBody] UpdateCommentRequestDTO dto,
        [FromServices] IMediator mediator)
    {
        var request = dto.Adapt<UpdateCommentRequest>();
        await mediator.Send(request);
        return Results.Ok();
    }
}

