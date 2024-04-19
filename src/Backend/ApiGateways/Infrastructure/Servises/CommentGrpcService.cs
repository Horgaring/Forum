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

    public async Task<CommentResponseGrpc> CreateSubCommentAsync(CreateSubCommentRequestGrpc request)
    {
        var resp = await _client.CreateSubCommentAsync(request);
        return resp;
    }
    public async Task<CommentResponseGrpc> CreateCommentAsync(CreateCommentRequestGrpc request)
    {
        var resp = await _client.CreateCommentAsync(request);
        return resp;
    }
    public async Task<bool> RemoveCommentAsync(DeleteCommentRequestGrpc request)
    {
        var code = await _client.DeleteCommentAsync(request);
        if (code == null) return false;
        if (code.Succes == true) return true;
        return false;
    }
    public async Task<bool> UpdateCommentAsync(UpdateCommentRequestGrpc request)
    {
        var code = await _client.UpdateCommentAsync(request);
        if (code == null) return false;
        if (code.Succes == true) return true;
        return false;
    }
    public async Task<List<CommentResponseGrpc>> GetCommentAsync(GetCommentRequestGrpc request)
    {
        var resp = await _client.GetCommentsAsync(request);
        return resp.ResponseDtos.ToList();
    }

}