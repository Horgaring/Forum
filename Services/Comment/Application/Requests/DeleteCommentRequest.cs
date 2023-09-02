using System.Net;
using Application.Exception;
using Domain;
using Grpc.Core;
using Infrastructure.Context;
using MediatR;

namespace Application.Requests;

public class DeleteCommentRequest: IRequest
{
    public int id { get; set; }
}
public class DeleteCommentHandler : IRequestHandler<DeleteCommentRequest>
{
    private readonly CommentDbContext _db;

    public DeleteCommentHandler(CommentDbContext db)=>
        (_db) = (db);


    public async Task Handle(DeleteCommentRequest request, CancellationToken cancellationToken)
    {
        var comment = _db.Comment.SingleOrDefault(comment => comment.Id == request.id);
        if (comment == null)
        {
            throw new CommentNotFound(StatusCode.NotFound,null);
        }
        _db.Comment.Remove(comment);
        await _db.SaveChangesAsync(cancellationToken);
    }
}


