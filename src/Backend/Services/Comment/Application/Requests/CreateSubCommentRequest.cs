using Application.Exception;
using BuildingBlocks.Core.Events.Comment;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Domain.ValueObjects;
using MassTransit;
using MediatR;

namespace Application.Requests;

public class CreateSubCommentRequest:  IRequest<SubComment>
{
    public CreateSubCommentRequest(string content, CustomerInfo customerInfo)
    {
        Content = content;
        CustomerInfo = customerInfo;
    }
    public CreateSubCommentRequest() { }
    public CreateSubCommentRequest(string content)
    {
        Content = content;
    }
    public Guid Postid { get; set; }
    public string Content { get; init; }
    public CustomerInfo CustomerInfo { get; set; }
    public Guid ParentComment { get; set; }
    
}
public class CreateSubCommentHandler : IRequestHandler<CreateSubCommentRequest,SubComment>
{
    private readonly IRepository<Comment, Guid> _repository;
    private readonly IUnitOfWork _uow;
    private readonly IPublishEndpoint _endpoint;

    public CreateSubCommentHandler(IRepository<Comment, Guid> repository, IUnitOfWork uow, IPublishEndpoint endpoint)
    {
        _repository = repository;
        _uow = uow;
        _endpoint = endpoint;
    }


    public async Task<SubComment> Handle(CreateSubCommentRequest request, CancellationToken cancellationToken)
    {
        
        var comment = new SubComment(request.Content)
        {
            Postid = request.Postid,
            CreatedAt = DateTime.UtcNow,
            LastUpdate = DateTime.UtcNow,
            ParentComment = request.ParentComment,
            CustomerInfo = request.CustomerInfo,
            
        };

        if (await _repository.GetByIdAsync(comment.ParentComment) == null) 
            throw new CommentNotFound(new []{"Parent Comment Id is Invalid"});
            
        comment.RaiseEvent(new CommentCreatedEvent()
        {
            Content = comment.Content,
            CustomerId = request.CustomerInfo.Id,
            Date = comment.CreatedAt,
            PostId = comment.Postid
        });
        await _repository.CreateAsync(comment);
        await _uow.CommitAsync(cancellationToken);
        
        return comment;
    }
}