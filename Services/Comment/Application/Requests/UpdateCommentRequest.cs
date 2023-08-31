using System.Net;
using Application.Exception;
using Infrastructure.Context;
using MediatR;

namespace Application.Requests;

public class UpdateCommentRequest: IRequest
{
    public UpdateCommentRequest(string content)
    {
        Content = content;
    }
    public int id { get; set; }
    public string Content { get; init; }
}
public class UpdateCommentHandler : IRequestHandler<UpdateCommentRequest>
{
    private readonly CommentDbContext _db;

    public UpdateCommentHandler(CommentDbContext db)=>
        (_db) = (db);


    public async Task Handle(UpdateCommentRequest request, CancellationToken cancellationToken)
    {
        var comment = _db.Comment.SingleOrDefault(comment => comment.Id == request.id);
        if (comment == null)
        {
            throw new CommentNotFound(HttpStatusCode.BadRequest,null);
        }
        comment.Update(request.Content);
        _db.Comment.Update(comment);
        await _db.SaveChangesAsync(cancellationToken);
    }
}


