using BuildingBlocks.Events.Comment;
using Domain;
using Infrastructure.Context;
using MassTransit;
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
    public CustomerInfo customerInfo { get; set; }
}
public class CreatePostHandler : IRequestHandler<CreateCommentRequest,Comment>
{
    private readonly CommentDbContext _db;
    private readonly IPublishEndpoint _endpoint;

    public CreatePostHandler(CommentDbContext db, IPublishEndpoint endpoint)
    {
        (_db) = (db);
        _endpoint = endpoint;
    }


    public async Task<Comment> Handle(CreateCommentRequest request, CancellationToken cancellationToken)
    {
        
        var comment = new Comment(request.Content)
        {
            Postid = request.Postid,
            Date = DateTime.UtcNow
        };
        _db.Comment.Add(comment);
        await _db.SaveChangesAsync(cancellationToken);
        await _endpoint.Publish<CommentCreatedEvent>(new CommentCreatedEvent()
        {
            Content = comment.Content,
            CustomerId = request.customerInfo.Id,
            CustomerName = request.customerInfo.Name,
            Date = comment.Date,
            PostId = comment.Postid
        }, cancellationToken);
        return comment;
    }
}