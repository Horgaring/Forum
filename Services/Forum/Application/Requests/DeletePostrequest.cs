using Application.Exceptions;
using BuildingBlocks.Core.Events.Post;
using BuildingBlocks.Core.Repository;
using Grpc.Core;
using Infrastructure.Context;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests;

public class DeletePostRequest : IRequest<bool>
{
    public Guid id { get; set; }
}

public class DeletePostHandler : IRequestHandler<DeletePostRequest, bool>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly PostRepository _repository;
    private readonly IUnitOfWork<PostRepository> _uow;

    public DeletePostHandler(PostRepository repository,IUnitOfWork<PostRepository> uow,IPublishEndpoint endp)=>
        (_repository,_uow,_publishEndpoint) = (repository,uow,endp);

    
    public async Task<bool> Handle(DeletePostRequest request, CancellationToken cancellationToken)
    {
        var post = await _repository.SingleOrDefaultAsync(op => op.Id == request.id);
        if (post is null)
        {
            throw new PostNotFountException(StatusCode.NotFound,null);
        }
        _repository.Delete(post);
        if (await _uow.CommitAsync() > 0)
        {
            await _publishEndpoint.Publish<DeletedPostEvent>(new DeletedPostEvent()
            {
                PostId = post.Id
            }, cancellationToken);
            return true;
        }
        return false;
    }
}