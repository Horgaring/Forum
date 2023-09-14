using Application.PostRequests;
using AutoMapper;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService1;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Api.Services;

public class PostService : Post.PostBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    public PostService(IMediator mediator,IMapper mapper)=>
        (_mediator,_mapper) = (mediator,mapper);

    public override async Task<PostResponseDTO> CreatePost(PostRequestDTO request, ServerCallContext context)
    {
        var createpost = new Application.PostRequests.CreatePostRequest()
        {
            Userid = context.GetHttpContext().User.Claims
                .First(op => op.Type ==  JwtClaimTypes.Subject).Value,
            Title = request.Title,
            Description = request.Description,
            Date = DateTime.UtcNow
        };
        var succes = await _mediator.Send(createpost);
        return  new PostResponseDTO()
        {
            Userid = createpost.Userid,
            Title = createpost.Title,
            Date = Timestamp.FromDateTime(createpost.Date),
            Description = createpost.Description
        };
    }

    public override async Task<PostsResponseDTO> GetPosts(GetPostRequestDTO request, ServerCallContext context)
    {
        var getPost = new Application.PostRequests.GetPostRequest()
        {
            Query = request.Query,
            PageNum = request.Pagenum,
            PageSize = request.Pagesize
        };
        var res = await _mediator.Send(getPost);
        var result = new PostsResponseDTO();
        result.ResponseDto.AddRange(res);
        return result;
    }

    public override async Task<StatusResponse> UpdatePost(PostRequestDTO request, ServerCallContext context)
    {
        var updatePost = new Application.PostRequests.UpdatePostRequest()
        {
            Userid = context.GetHttpContext().User.Claims
                .First(op => op.Type ==  JwtClaimTypes.Subject).Value,
            Title = request.Title,
            Description = request.Description,
        };
        await _mediator.Send(updatePost);
        return await Task.FromResult(new StatusResponse(){Succes = true});
    }

    public override async Task<StatusResponse> DeletePost(DeletePostRequestDTO request, ServerCallContext context)
    {
        var deletepost = new Application.PostRequests.DeletePostRequest()
        {
            id = Guid.Parse(request.Id)
        };
        var succes = await _mediator.Send(deletepost);
        return new StatusResponse(){Succes = succes};
    }
}