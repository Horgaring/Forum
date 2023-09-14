using Application.Requests;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService1;
using MediatR;
using GetCommentRequest = GrpcService1.GetCommentRequest;

namespace Api.Services;

public class CommentService : GrpcService1.Comment.CommentBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    public CommentService(IMediator mediator,IMapper mapper)=>
        (_mediator,_mapper) = (mediator,mapper);
    public async override Task<CommentResponseDTO> CreateComment(CommentRequestDTO commentdto, ServerCallContext context)
    {
        var request =  _mapper.Map<CreateCommentRequest>(commentdto);
        var comment = await _mediator.Send(request);
        return new CommentResponseDTO()
        {
            Content = comment.Content,
            Date = Timestamp.FromDateTime(comment.Date),
            Postid = comment.Postid.ToString()
        };
    }

    public async override Task<StatusResponse> DeleteComment(DeleteCommentRequestDTO commentdto, ServerCallContext context)
    {
        var request =  _mapper.Map<DeleteCommentRequest>(commentdto);
        await _mediator.Send(request);
        return new StatusResponse(){Succes = true};
    }

    
}