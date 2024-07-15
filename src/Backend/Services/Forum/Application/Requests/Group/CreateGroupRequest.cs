using Application.Exceptions.Common;
using BuildingBlocks.Core.Repository;
using Domain.Entities;

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
    
    public byte[] Avatar { get; set; }
}

public class CreateGroupRequestHandler : IRequestHandler<CreateGroupRequest>
{
    private readonly IRepository<Domain.Entities.Group, Guid> _repository;
    
    private readonly IRepository<CustomerId, Guid> _custrepository;
    
    private readonly IUnitOfWork _uow;

    public CreateGroupRequestHandler(IRepository<Domain.Entities.Group, Guid> repository,IUnitOfWork uow, IRepository<CustomerId, Guid> custrepository)
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
        group.Followers = (new List<CustomerId>());
        group.Followers.Add(userid);
        await _repository.CreateAsync(group);
        await _uow.CommitAsync();
    }
}