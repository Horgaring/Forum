using Application.Exceptions.Common;
using Application.Exceptions.Group;
using BuildingBlocks.Core.Repository;
using Infrastructure.Context;
using Infrastructure.Context.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests;

public class UpdateGroupRequest : IRequest
{
    public string Name { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public byte[] Avatar { get; set; }
}

public class UpdateGroupRequestHandler : IRequestHandler<UpdateGroupRequest>
{
    private readonly GroupRepository _repository;
    
    private readonly CustomerIdRepository _customerRepository;
    
    private readonly IUnitOfWork<PostDbContext> _uow;

    public UpdateGroupRequestHandler(GroupRepository repository,IUnitOfWork<PostDbContext> uow, CustomerIdRepository customerRepository)
    {
        _customerRepository = customerRepository;
        (_repository, _uow) = (repository, uow);
    }

    public async Task Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
    {
        var group = await _repository.Table.Include(p => p.Owner).FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken: cancellationToken) ?? throw new GroupNotFoundExeption();
        if (group.Owner != (await _customerRepository.GetByIdAsync(request.CustomerId)
                            ?? throw new CustomerNotFound()))
        {
            throw new PermissionDenied();
        }
        
        group.Name = request.Name;
        
        _repository.Update(group);
        await _uow.CommitAsync();
    }
}