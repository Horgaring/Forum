using Application.DTOs.Common;
using Application.DTOs.Group;
using Application.Exceptions.Group;
using Infrastructure.Context.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Group;

public class GetGroupsRequest : IRequest<List<GroupDto>>
{
    public string Name { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
}
public class GetGroupsRequestHandler : IRequestHandler<GetGroupsRequest,List<GroupDto>>
{
    private readonly GroupRepository _repository;

    public GetGroupsRequestHandler(GroupRepository repository)=>
        (_repository) = (repository);
    
    public async Task<List<GroupDto>> Handle(GetGroupsRequest request, CancellationToken cancellationToken)
    {
        if ( await _repository.Where(p => p.Name.Contains(request.Name))
                .Skip(request.Size * (request.Page - 1))
                .Take(request.Page)
                .AsNoTracking()
                .Select(p => new GroupDto(p.Followers.Count,
                    new AcountDto(p.Owner.Name,p.Owner.Id),
                    p.Name,
                    p.Id,
                    p.AvatarPath)).ToListAsync() is List<GroupDto> groups)
        {
            return groups;
        }
        throw new GroupNotFoundExeption();
    }
}