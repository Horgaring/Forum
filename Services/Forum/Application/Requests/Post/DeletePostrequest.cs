using Application.Exceptions;
using Application.Exceptions.Common;
using Application.Exceptions.Post;
using BuildingBlocks.Core.Events.Post;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Grpc.Core;
using Infrastructure.Context;
using MassTransit;
using MediatR;

namespace Application.Requests.Post;

public class DeletePostRequest : IRequest<bool>
{
    public CustomerId User { get; set; }
    public Guid id { get; set; }
}

public class DeletePostHandler : IRequestHandler<DeletePostRequest, bool>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly PostRepository _repository;
    private readonly IUnitOfWork<PostDbContext> _uow;

    public DeletePostHandler(PostRepository repository,IUnitOfWork<PostDbContext> uow,IPublishEndpoint endp)=>
        (_repository,_uow,_publishEndpoint) = (repository,uow,endp);

    
    public async Task<bool> Handle(DeletePostRequest request, CancellationToken cancellationToken)
    {
        var post = await _repository.SingleOrDefaultAsync(op => op.Id == request.id);
        
        if (post is null)
        {
            throw new PostNotFountException(null);
        }

        if (request.User != post.User)
        {
            throw new PermissionDenied(new[]{"User is not have this post"});
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