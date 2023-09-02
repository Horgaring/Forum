using Application.Requests;
using AutoMapper;
using Grpc.Core;
using GrpcService1;
using MediatR;
using GetCommentRequest = GrpcService1.GetCommentRequest;

namespace Api.Services;

public class CommentService : Comment.CommentBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    public CommentService(IMediator mediator,IMapper mapper)=>
        (_mediator,_mapper) = (mediator,mapper);
    public async override Task<StatusResponse> CreateComment(CommentRequestDTO commentdto, ServerCallContext context)
    {
        var request =  _mapper.Map<CreateCommentRequest>(commentdto);
        await _mediator.Send(request);
        return new StatusResponse(){Succes = true};
    }

    public async override Task<StatusResponse> DeleteComment(DeleteCommentRequestDTO commentdto, ServerCallContext context)
    {
        var request =  _mapper.Map<DeleteCommentRequest>(commentdto);
        await _mediator.Send(request);
        return new StatusResponse(){Succes = true};
    }

    public override Task<CommentsResponse> GetComments(GetCommentRequest request, ServerCallContext context)
    {
        return base.GetComments(request, context);
    }

    public override Task<UpdateResult> UpdateComment(CommentRequestDTO request, ServerCallContext context)
    {
        return base.UpdateComment(request, context);
    }
}