using Application.Exceptions.Common;
using Application.Exceptions.Group;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Infrastructure.Context;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Group;

public class LeaveFromGroupRequest : IRequest
{
    public Guid CustomerId { get; set; }
    public string Name { get; set; }
}
public class LeaveFromGroupRequestHandler : IRequestHandler<LeaveFromGroupRequest>
{
    
    private readonly GroupRepository _repository;
    
    private readonly CustomerIdRepository _customerRepository;
    
    private readonly IUnitOfWork<PostDbContext> _uow;

    public LeaveFromGroupRequestHandler(GroupRepository repository, IUnitOfWork<PostDbContext> uow, CustomerIdRepository customerRepository)
    {
        (_repository) = (repository);
        _uow = uow;
        _customerRepository = customerRepository;
    }

    public async Task Handle(LeaveFromGroupRequest request, CancellationToken cancellationToken)
    {
        var group =  _repository.Table.Include(p => p.Followers)
            .FirstOrDefault(p => p.Name == request.Name) ?? throw new GroupNotFoundExeption();

        group.Followers.RemoveAll(p => p.Id == request.CustomerId);
        
        await _uow.CommitAsync();
    }
}