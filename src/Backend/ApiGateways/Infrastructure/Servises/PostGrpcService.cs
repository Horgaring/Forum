using Application.DTOs;
using GrpcClientpost;
using Infrastructure.Options;

namespace Infrastructure.Clients;

public class PostGrpcService
{
    private readonly Post.PostClient _client;

    public PostGrpcService(GrpcClientpost.Post.PostClient client)
    {
        _client = client;
    }

    public async Task<PostResponseGrpc?> CreatePostAsync(PostRequestGrpc request)
    {
       var resp = await _client.CreatePostAsync(request);
       return resp;
    }
    public async Task<bool> RemovePostAsync(PostRequestIdGrpc request)
    {
       var code = await _client.DeletePostAsync(request);
       if (code == null) return false;
       if (code.Succes == true) return true;
       return false;
    }
    public async Task<bool> UpdatePostAsync(PostRequestGrpc request)
    {
        var code = await _client.UpdatePostAsync(request);
        if (code == null) return false;
        if (code.Succes == true) return true;
        return false;
    }
    public async Task<List<PostResponseGrpc?>> GetPostAsync(GetPostRequestGrpc request)
    {
        var resp = await _client.GetPostsAsync(request);
        if (resp == null) return Enumerable.Empty<PostResponseGrpc?>().ToList();
        return resp.ResponseDto.ToList();
    }
    
}