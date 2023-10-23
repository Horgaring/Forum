using BuildingBlocks.Events.Comment;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Context;
using MassTransit;
using MediatR;

namespace Application.Requests;

public class CreateSubCommentRequest: IRequest<SubComment>
{
    public CreateSubCommentRequest(string content)
    {
        Content = content;
    }
    public Guid Postid { get; set; }
    public string Content { get; init; }
    public int ParentComment { get; set; }
    public CustomerInfo customerInfo { get; set; }
}
public class CreateSubCommentHandler : IRequestHandler<CreateSubCommentRequest,SubComment>
{
    private readonly CommentDbContext _db;
    private readonly IPublishEndpoint _endpoint;

    public CreateSubCommentHandler(CommentDbContext db, IPublishEndpoint endpoint)
    {
        (_db) = (db);
        _endpoint = endpoint;
    }


    public async Task<SubComment> Handle(CreateSubCommentRequest request, CancellationToken cancellationToken)
    {
        
        var comment = new SubComment(request.Content)
        {
            Postid = request.Postid,
            Date = DateTime.UtcNow,
            ParentComment = request.ParentComment
        };
        _db.SubComment.Add(comment);
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