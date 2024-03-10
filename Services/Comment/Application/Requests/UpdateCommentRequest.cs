using System.Net;
using Application.Exception;
using BuildingBlocks.Core.Repository;
using Grpc.Core;
using Infrastructure.Context;
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
    private readonly CommentRepository _repository;
    private readonly IUnitOfWork<CommentDbContext> _uow;

    public UpdateCommentHandler(CommentRepository repository, IUnitOfWork<CommentDbContext> uow)
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


