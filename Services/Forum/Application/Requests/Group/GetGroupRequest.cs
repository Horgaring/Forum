using Application.Exceptions;
using Application.Exceptions.Group;
using Infrastructure.Context;
using MediatR;

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
        if (await _repository.GetByIdAsync(request.Id) is Domain.Entities.Group group)
        {
            return group;
        }
        throw new GroupNotFoundExeption();
    }
}