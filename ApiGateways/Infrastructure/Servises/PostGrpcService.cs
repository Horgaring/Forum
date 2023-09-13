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

    public async Task<PostResponseDTO?> CreatePostAsync(PostRequestDTO request)
    {
       var resp = await _client.CreatePostAsync(request);
       if (resp == null)
       {
           return null;
       }
       return resp;
    }
    public async Task<bool> RemovePostAsync(DeletePostRequestDTO request)
    {
       var code = await _client.DeletePostAsync(request);
       if (code == null) return false;
       if (code.Succes == true) return true;
       return false;
    }
    public async Task<bool> UpdatePostAsync(PostRequestDTO request)
    {
        var code = await _client.UpdatePostAsync(request);
        if (code == null) return false;
        if (code.Succes == true) return true;
        return false;
    }
    public async Task<List<PostResponseDTO?>> GetPostAsync(GetPostRequestDTO request)
    {
        var resp = await _client.GetPostsAsync(request);
        if (resp == null) return Enumerable.Empty<PostResponseDTO?>().ToList();
        return resp.ResponseDto.ToList();
    }
    
}