using Domain;
using Infrastructure.Context;
using MediatR;

namespace Application.Requests;

public class CreateCommentRequest: IRequest<Comment>
{
    public CreateCommentRequest(string content)
    {
        Content = content;
    }
    public Guid Postid { get; set; }
    public string Content { get; init; }
}
public class CreatePostHandler : IRequestHandler<CreateCommentRequest,Comment>
{
    private readonly CommentDbContext _db;

    public CreatePostHandler(CommentDbContext db)=>
        (_db) = (db);


    public async Task<Comment> Handle(CreateCommentRequest request, CancellationToken cancellationToken)
    {
        
        var comment = new Comment(request.Content)
        {
            Postid = request.Postid,
            Date = DateTime.UtcNow
        };
        _db.Comment.Add(comment);
        await _db.SaveChangesAsync(cancellationToken);
        return comment;
    }
}