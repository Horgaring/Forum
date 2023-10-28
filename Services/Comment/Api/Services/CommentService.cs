using Application.Requests;
using AutoMapper;
using Domain;
using Domain.ValueObjects;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService1;
using IdentityModel;
using Mapster;
using MediatR;

namespace Api.Services;

public class CommentService : GrpcService1.Comment.CommentBase
{
    
    private readonly IMediator _mediator;
    
    public CommentService(IMediator mediator,IMapper mapper)=>
        (_mediator) = (mediator);

    public override async Task<CommentResponseGrpc> CreateSubComment(CreateSubCommentRequestGrpc commentdto, ServerCallContext context)
    {
        var httpcontext = context.GetHttpContext();
        var request =  commentdto.Adapt<CreateSubCommentRequest>();
        request.customerInfo = new CustomerInfo()
        {
            Id = httpcontext.User.Claims.First(p => p.Type == JwtClaimTypes.Subject).Value,
            Name = httpcontext.User.Claims.First(p => p.Type == JwtClaimTypes.Name).Value
        };
        var comment = await _mediator.Send(request);
        return new CommentResponseGrpc()
        {
            Content = comment.Content,
            Date = Timestamp.FromDateTime(comment.Date),
            Postid = comment.Postid.ToString()
        };
    }

    public async override Task<CommentResponseGrpc> CreateComment(CreateCommentRequestGrpc commentdto, ServerCallContext context)
    {
        var httpcontext = context.GetHttpContext();
        var request =  commentdto.Adapt<CreateCommentRequest>();
        request.customerInfo = new CustomerInfo()
        {
            Id = httpcontext.User.Claims.First(p => p.Type == JwtClaimTypes.Subject).Value,
            Name = httpcontext.User.Claims.First(p => p.Type == JwtClaimTypes.Name).Value
        };
        var comment = await _mediator.Send(request);
        return new CommentResponseGrpc()
        {
            Content = comment.Content,
            Date = Timestamp.FromDateTime(comment.Date),
            Postid = comment.Postid.ToString()
        };
    }

    public async override Task<StatusResponse> DeleteComment(DeleteCommentRequestGrpc commentdto, ServerCallContext context)
    {
        var request =  commentdto.Adapt<DeleteCommentRequest>();
        await _mediator.Send(request);
        return new StatusResponse(){Succes = true};
    }
    public override async Task<CommentsResponse> GetComments(GetCommentRequestGrpc request, ServerCallContext context)
    {
        var query = request.Adapt<Application.Requests.GetCommentRequest>();
        var result = new CommentsResponse();
        result.ResponseDtos.AddRange(await _mediator.Send(query));
        return result;
    }

    public override async Task<StatusResponse> UpdateComment(UpdateCommentRequestGrpc request, ServerCallContext context)
    {
        var command =  request.Adapt<Application.Requests.GetCommentRequest>();
        var result = await _mediator.Send(command);
        return new StatusResponse(){Succes = true};
    }
}