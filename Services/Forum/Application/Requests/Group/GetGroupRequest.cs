using Application.DTOs.Common;
using Application.DTOs.Group;
using Application.Exceptions.Group;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Group;

public class GetGroupRequest : IRequest<Domain.Entities.Group>
{
    public Guid Id { get; set; }
}
public class GetGroupRequestHandler : IRequestHandler<GetGroupRequest,Domain.Entities.Group>
{
    private readonly GroupRepository _repository;

    public GetGroupRequestHandler(GroupRepository repository)=>
        (_repository) = (repository);
    
    public async Task<Domain.Entities.Group> Handle(GetGroupRequest request, CancellationToken cancellationToken)
    {
        var group = await _repository.GetByIdAsync(request.Id);

        if (group == null)
        {
            throw new GroupNotFoundExeption();
        }

        return group;
    }
}