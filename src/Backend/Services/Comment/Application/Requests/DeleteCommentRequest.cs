using System.Net;
using Application.Exception;
using BuildingBlocks.Core.Repository;
using Domain;
using Domain.Entities;
using Grpc.Core;
using MassTransit;
using MediatR;

namespace Application.Requests;

public class DeleteCommentRequest: IRequest
{
    public Guid id { get; set; }
}
public class DeleteCommentHandler : IRequestHandler<DeleteCommentRequest>
{
    private readonly IRepository<Comment, Guid> _repository;
    private readonly IUnitOfWork _uow;

    public DeleteCommentHandler(IRepository<Comment, Guid> repository, IUnitOfWork uow, IPublishEndpoint endpoint)
    {
        _repository = repository;
        _uow = uow;
    }


    public async Task Handle(DeleteCommentRequest request, CancellationToken cancellationToken)
    {
        var comment = await _repository.SingleOrDefaultAsync(comment => comment.Id == request.id);
        if (comment == null)
        {
            throw new CommentNotFound(null);
        }
        _repository.Delete(comment);
        await _uow.CommitAsync(cancellationToken);
    }
}


