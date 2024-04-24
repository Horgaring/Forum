using Application.Exceptions;
using Application.Exceptions.Group;
using Application.Exceptions.Post;
using BuildingBlocks.Core.Events.Post;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Grpc.Core;
using Infrastructure.Context;
using Infrastructure.Context.Repository;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Requests.Post;

public class CreatePostRequest : IRequest<Domain.Entities.Post>
{
    public Guid GroupId { get; set; }
    public CustomerId User { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}
public class CreatePostHandler : IRequestHandler<CreatePostRequest,Domain.Entities.Post>
{
    private readonly PostRepository _repository;
    private readonly GroupRepository _grouprepository;
    private readonly CustomerIdRepository _customerIdRepository;
    
    private readonly IUnitOfWork<PostDbContext> _uow;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreatePostHandler(PostRepository repository,IUnitOfWork<PostDbContext> uow, IPublishEndpoint publishEndpoint, GroupRepository grouprepository, CustomerIdRepository h, CustomerIdRepository customerIdRepository)
    {
        _publishEndpoint = publishEndpoint;
        _grouprepository = grouprepository;
        _customerIdRepository = customerIdRepository;

        (_repository, _uow,_publishEndpoint) = (repository, uow,publishEndpoint);
    }


    public async Task<Domain.Entities.Post> Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var user = await _customerIdRepository.Table.FirstOrDefaultAsync(p => p == request.User);
        
        var postentity = request.Adapt<Domain.Entities.Post>();
        postentity.User = user;
        var req = await _grouprepository.Where(p => p.Id == request.GroupId &&
            p.Followers.Contains(user))
            .FirstOrDefaultAsync() ?? throw new GroupNotFoundExeption();
        postentity.Group = req;
        postentity.RaiseEvent(postentity.Adapt<CreatedPostEvent>());
        
        await _repository.CreateAsync(postentity);
        await _uow.CommitAsync();
        
        return postentity;
    }
}