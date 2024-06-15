using Application.DTOs.Common;
using Application.DTOs.Group;
using Application.Exceptions.Group;
using Infrastructure.Context.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Group;

public class GetGroupRequest : IRequest<GroupDto>
{
    public Guid Id { get; set; }
}
public class GetGroupRequestHandler : IRequestHandler<GetGroupRequest,GroupDto>
{
    private readonly GroupRepository _repository;

    public GetGroupRequestHandler(GroupRepository repository)=>
        (_repository) = (repository);
    
    public async Task<GroupDto> Handle(GetGroupRequest request, CancellationToken cancellationToken)
    {
        var group = await _repository.Table
            .Where(p => p.Id == request.Id)
            .Include(p => p.Followers)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (group == null)
        {
            throw new GroupNotFoundExeption();
        }
        var groupdto = new GroupDto(group.Followers.Count,
            new AccountDto(group.Owner.Name,group.Owner.Id),
            group.Name,
            group.Id,
            group.AvatarPath);
        return groupdto;
    }
}