using Application.Exceptions;
using Application.Exceptions.Group;
using Application.Exceptions.Post;
using BuildingBlocks.Core.Events.Post;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Grpc.Core;
using Infrastructure.Context;
using Mapster;
using MassTransit;
using MediatR;

namespace Application.Requests.Post;

public class CreatePostRequest : IRequest<Domain.Entities.Post>
{
    public Guid GroupId { get; set; }
    public CustomerId Userid { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}
public class CreatePostHandler : IRequestHandler<CreatePostRequest,Domain.Entities.Post>
{
    private readonly PostRepository _repository;
    private readonly GroupRepository _grouprepository;
    private readonly IUnitOfWork<PostDbContext> _uow;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreatePostHandler(PostRepository repository,IUnitOfWork<PostDbContext> uow, IPublishEndpoint publishEndpoint, GroupRepository grouprepository)
    {
        _publishEndpoint = publishEndpoint;
        _grouprepository = grouprepository;
        (_repository, _uow,_publishEndpoint) = (repository, uow,publishEndpoint);
    }


    public async Task<Domain.Entities.Post> Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var postentity = request.Adapt<Domain.Entities.Post>();
        
        if (await _repository.SingleOrDefaultAsync(post => post.Userid == postentity.Userid
                                                           && post.Title == postentity.Title) != null)
        {
            throw new PostAlreadyExistException(null);
        }

        var req = await _grouprepository.GetByIdAsync(request.GroupId) ?? throw new GroupNotFoundExeption();
        req.Posts.Add(postentity);
        await _repository.CreateAsync(postentity);
        await _uow.CommitAsync();
        await _publishEndpoint.Publish<CreatedPostEvent>(postentity.Adapt<CreatedPostEvent>());
        return postentity;
    }
}