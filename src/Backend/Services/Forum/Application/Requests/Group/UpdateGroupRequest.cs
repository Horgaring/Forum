using Application.Exceptions.Common;
using Application.Exceptions.Group;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using MediatR;
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
    private readonly IRepository<Domain.Entities.Group, Guid> _repository;
    
    private readonly IRepository<CustomerId, Guid> _customerRepository;
    
    private readonly IUnitOfWork _uow;

    public UpdateGroupRequestHandler(IRepository<Domain.Entities.Group, Guid> repository,IUnitOfWork uow, IRepository<CustomerId, Guid> customerRepository)
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