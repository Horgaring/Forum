using Application.Exceptions.Common;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Context.Repository;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Requests;

public class CreateGroupRequest : IRequest
{
    public CustomerId User { get; set; }
    public string Name { get; set; }
    
    public IFormFile Avatar { get; set; }
}

public class CreateGroupRequestHandler : IRequestHandler<CreateGroupRequest>
{
    private readonly GroupRepository _repository;
    
    private readonly CustomerIdRepository _custrepository;
    
    private readonly IUnitOfWork<PostDbContext> _uow;

    public CreateGroupRequestHandler(GroupRepository repository,IUnitOfWork<PostDbContext> uow, CustomerIdRepository custrepository)
    {
        _custrepository = custrepository;
        (_repository, _uow) = (repository, uow);
    }

    public async Task Handle(CreateGroupRequest request, CancellationToken cancellationToken)
    {
        
        var group = request.Adapt<Domain.Entities.Group>();
        var userid = _custrepository.Table.FirstOrDefault(p => p.Id == request.User.Id);

        if (userid == null)
        {
            throw new CustomerNotFound();
        }
        group.Owner = userid;
        
        using (MemoryStream fs = new())
        {
            request.Avatar.CopyTo(fs);
            group.AvatarPath = fs.ToArray();
            
        }
        group.Followers = (new List<CustomerId>());
        group.Followers.Add(userid);
        await _repository.CreateAsync(group);
        await _uow.CommitAsync();
    }
}