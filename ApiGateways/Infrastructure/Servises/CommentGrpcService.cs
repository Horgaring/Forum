using Grpc.Core;
using GrpcClientcomment;
using GrpcClientpost;
using Infrastructure.Options;

namespace Infrastructure.Clients;

public class CommentGrpcService
{
    private readonly Comment.CommentClient _client;

    public CommentGrpcService(Comment.CommentClient client)
    {
        _client = client;
    }

    public async Task<CommentResponseDTO> CreateCommentAsync(CommentRequestDTO request)
    {
        var resp = await _client.CreateCommentAsync(request);
        return resp;
    }
    public async Task<bool> RemoveCommentAsync(DeleteCommentRequestDTO request)
    {
        var code = await _client.DeleteCommentAsync(request);
        if (code == null) return false;
        if (code.Succes == true) return true;
        return false;
    }
    public async Task<bool> UpdateCommentAsync(CommentRequestDTO request)
    {
        var code = await _client.UpdateCommentAsync(request);
        if (code == null) return false;
        if (code.Succes == true) return true;
        return false;
    }
    public async Task<List<CommentResponseDTO>> GetCommentAsync(GetCommentRequest request)
    {
        var resp = await _client.GetCommentsAsync(request);
        return resp.ResponseDtos.ToList();
    }

}