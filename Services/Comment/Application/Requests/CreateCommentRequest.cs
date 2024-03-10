using BuildingBlocks.Core.Events.Comment;
using BuildingBlocks.Core.Repository;
using Domain;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Context;
using MassTransit;
using MediatR;

namespace Application.Requests;

public class CreateCommentRequest: IRequest<Comment>
{
    public CreateCommentRequest(string content, CustomerInfo customerInfo)
    {
        Content = content;
        CustomerInfo = customerInfo;
    }
    public CreateCommentRequest() { }
    public CreateCommentRequest(string content)
    {
        Content = content;
    }
    public Guid Postid { get; set; }
    public string Content { get; init; }
    public CustomerInfo CustomerInfo { get; set; }
}
public class CreateCommentHandler : IRequestHandler<CreateCommentRequest,Comment>
{
    private readonly CommentRepository _repository;
    private readonly IUnitOfWork<CommentDbContext> _uow;
    private readonly IPublishEndpoint _endpoint;

    public CreateCommentHandler(CommentRepository repository, IUnitOfWork<CommentDbContext> uow, IPublishEndpoint endpoint)
    {
        _repository = repository;
        _uow = uow;
        _endpoint = endpoint;
    }

    public async Task<Comment> Handle(CreateCommentRequest request
        , CancellationToken cancellationToken)
    {
        
        var comment = new Comment(request.Content)
        {
            Postid = request.Postid,
            CreatedAt = DateTime.UtcNow,
            CustomerInfo = request.CustomerInfo,
            LastUpdate = DateTime.UtcNow,
            
        };
        await _repository.CreateAsync(comment);
        await _uow.CommitAsync(cancellationToken);
        await _endpoint.Publish<CommentCreatedEvent>(new CommentCreatedEvent()
        {
            Content = comment.Content,
            CustomerId = request.CustomerInfo.Id,
            Date = comment.CreatedAt,
            PostId = comment.Postid
        }, cancellationToken);
        return comment;
    }
}
