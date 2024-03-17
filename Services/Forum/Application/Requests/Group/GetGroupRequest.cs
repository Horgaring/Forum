
using Application.DTOs;
using Application.DTOs.Common;
using Application.DTOs.Group;
using Application.Exceptions.Group;
using Domain.Entities;
using Infrastructure.Context;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Group;

public class GetGroupRequest : IRequest<List<GroupDto>>
{
    public string Name { get; set; }
}
public class GetGroupRequestHandler : IRequestHandler<GetGroupRequest,List<GroupDto>>
{
    private readonly GroupRepository _repository;

    public GetGroupRequestHandler(GroupRepository repository)=>
        (_repository) = (repository);
    
    public async Task<List<GroupDto>> Handle(GetGroupRequest request, CancellationToken cancellationToken)
    {
        if ( await _repository.Where(p => p.Name.Contains(request.Name))
                .Select(p => new GroupDto(p.Followers.Count,p.Posts,new AcountDto(p.Owner.Name),p.Name,p.Id)).ToListAsync() is List<GroupDto> groups)
        {
            return groups;
        }
        throw new GroupNotFoundExeption();
    }
}