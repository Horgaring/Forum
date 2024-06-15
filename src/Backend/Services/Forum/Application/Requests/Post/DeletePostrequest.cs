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
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Post;

public class DeletePostRequest : IRequest
{
    public CustomerId User { get; set; }
    public Guid id { get; set; }
}

public class DeletePostHandler : IRequestHandler<DeletePostRequest>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly PostRepository _repository;
    private readonly IUnitOfWork<PostDbContext> _uow;

    public DeletePostHandler(PostRepository repository,IUnitOfWork<PostDbContext> uow,IPublishEndpoint endp)=>
        (_repository,_uow,_publishEndpoint) = (repository,uow,endp);

    
    public async Task Handle(DeletePostRequest request, CancellationToken cancellationToken)
    {
        var post = await _repository.Table
            .Where(p => p.Id == request.id)
            .Include(p => p.User)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if (post is null)
        {
            throw new PostNotFountException(null);
        }

        if (request.User.Id != post.User.Id)
        {
            throw new PermissionDenied(new[]{"User does not have this post"});
        }
        post.RaiseEvent(new DeletedPostEvent()
            {
                PostId = post.Id
            });
        _repository.Delete(post);
        await _uow.CommitAsync();
        
    }
}