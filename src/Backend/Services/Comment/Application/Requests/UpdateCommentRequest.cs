using System.Net;
using Application.Exception;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using MediatR;

namespace Application.Requests;

public class UpdateCommentRequest: IRequest
{
    public UpdateCommentRequest() { }
    public UpdateCommentRequest(string content)
    {
        Content = content;
    }
    public Guid id { get; set; }
    public string Content { get; init; }
}
public class UpdateCommentHandler : IRequestHandler<UpdateCommentRequest>
{
    private readonly IRepository<Comment, Guid> _repository;
    private readonly IUnitOfWork _uow;

    public UpdateCommentHandler(IRepository<Comment, Guid> repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(UpdateCommentRequest request, CancellationToken cancellationToken)
    {
        var comment = await _repository.SingleOrDefaultAsync(comment => comment.Id == request.id);
        if (comment == null)
        {
            throw new CommentNotFound(null);
        }
        comment.Update(request.Content);
        _repository.Update(comment);
        await _uow.CommitAsync(cancellationToken);
    }
}


