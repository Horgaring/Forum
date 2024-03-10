using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Requests;

public class CreateGroupRequest : IRequest
{
    public CustomerId Userid { get; set; }
    public string Name { get; set; }
    
    public IFormFile Avatar { get; set; }
}

public class CreateGroupRequestHandler : IRequestHandler<CreateGroupRequest>
{
    private readonly GroupRepository _repository;
    
   
    
    private readonly IUnitOfWork<PostDbContext> _uow;

    public CreateGroupRequestHandler(GroupRepository repository,IUnitOfWork<PostDbContext> uow)
    {
        (_repository, _uow) = (repository, uow);
    }

    public async Task Handle(CreateGroupRequest request, CancellationToken cancellationToken)
    {
        
        var group = request.Adapt<Domain.Entities.Group>();
        group.Owner = request.Userid;
        using (MemoryStream fs = new())
        {
            request.Avatar.CopyTo(fs);
            //group.AvatarPath = fs.ToArray();
        }
        group.Followers.Add(request.Userid);
        await _repository.CreateAsync(group);
        await _uow.CommitAsync();
    }
}