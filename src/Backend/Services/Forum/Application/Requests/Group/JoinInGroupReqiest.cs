using Application.Exceptions.Common;
using Application.Exceptions.Group;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Context.Repository;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Group;

public class JoinInGroupReqiest : IRequest
{
    public CustomerId User { get; set; }
    
    public string Name { get; set; }
}

public class JoinInGroupReqiestHandler : IRequestHandler<JoinInGroupReqiest>
{
    
    private readonly CustomerIdRepository _customerRepository;
    private readonly GroupRepository _repository;
    private readonly IUnitOfWork<PostDbContext> _uow;

    public JoinInGroupReqiestHandler( IUnitOfWork<PostDbContext> uow, CustomerIdRepository customerRepository, GroupRepository repository)
    {
        _repository = repository;
        _uow = uow;
        _customerRepository = customerRepository;
    }
    
    public async Task Handle(JoinInGroupReqiest request, CancellationToken cancellationToken)
    {
        var group = await _repository.Table
            .Where(p => p.Name == request.Name)
            .Include(p => p.Followers)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new GroupNotFoundExeption();
        var userid = await _customerRepository.Table.FirstOrDefaultAsync(p => p.Id == request.User.Id, cancellationToken: cancellationToken);

        if (userid == null)
        {
            throw new CustomerNotFound();
        }
        group.Followers.Add(userid);
        
        await _uow.CommitAsync();
    }
}