using Application.Exceptions.Common;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Group;

public class JoinInGroupReqiest : IRequest
{
    public CustomerId User { get; set; }
    
    public Guid Id { get; set; }
}

public class JoinInGroupReqiestHandler : IRequestHandler<JoinInGroupReqiest>
{
    
    private readonly CustomerIdRepository _customerRepository;
    private readonly IUnitOfWork<PostDbContext> _uow;

    public JoinInGroupReqiestHandler( IUnitOfWork<PostDbContext> uow, CustomerIdRepository customerRepository)
    {
        
        _uow = uow;
        _customerRepository = customerRepository;
    }
    
    public async Task Handle(JoinInGroupReqiest request, CancellationToken cancellationToken)
    {
        var group = request.Adapt<Domain.Entities.Group>();
        var userid = await _customerRepository.Table.FirstOrDefaultAsync(p => p.Id == request.User.Id, cancellationToken: cancellationToken);

        if (userid == null)
        {
            throw new CustomerNotFound();
        }
        group.Followers.Add(userid);
        
        await _uow.CommitAsync();
    }
}