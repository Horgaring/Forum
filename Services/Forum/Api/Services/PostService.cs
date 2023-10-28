using Application.PostRequests;
using Mapster;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService1;
using IdentityModel;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Api.Services;

public class PostService : Post.PostBase
{
    private readonly IMediator _mediator;
    
    
    public PostService(IMediator mediator)=>
        (_mediator) = (mediator);

    public override async Task<PostResponseGrpc> CreatePost(PostRequestGrpc request, ServerCallContext context)
    {

        var createpost = request.Adapt<Application.PostRequests.CreatePostRequest>();
        createpost.Userid = context.GetHttpContext().User.Claims
            .First(op => op.Type == JwtClaimTypes.Subject).Value;
        createpost.Date = DateTime.UtcNow;
        var result = await _mediator.Send(createpost);
        var postResponseDTO = result.Adapt<PostResponseGrpc>();
        postResponseDTO.Date = Timestamp.FromDateTime(result.Date);
        return postResponseDTO;
    }

    public override async Task<PostsResponseGrpc> GetPosts(GetPostRequestGrpc request, ServerCallContext context)
    {
        var getPost = request.Adapt<GetPostRequest>();
        var res = await _mediator.Send(getPost);
        var result = new PostsResponseGrpc();
        result.ResponseDto.AddRange(res);
        return result;
    }

    public override async Task<StatusResponse> UpdatePost(PostRequestGrpc request, ServerCallContext context)
    {
        var updatePost = request.Adapt<Application.PostRequests.UpdatePostRequest>();
        updatePost.Userid = context.GetHttpContext().User.Claims
            .First(op => op.Type ==  JwtClaimTypes.Subject).Value;
        await _mediator.Send(updatePost);
        return new StatusResponse(){Succes = true};
    }

    public override async Task<StatusResponse> DeletePost(DeletePostRequestGrpc request, ServerCallContext context)
    {
        var deletepost = new Application.PostRequests.DeletePostRequest()
        {
            id = Guid.Parse(request.Id)
        };
        var succes = await _mediator.Send(deletepost);
        return new StatusResponse(){Succes = succes};
    }
}