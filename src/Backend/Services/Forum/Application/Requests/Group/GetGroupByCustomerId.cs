using Application.DTOs.Common;
using Application.DTOs.Group;
using Application.Exceptions.Common;
using BuildingBlocks.Core.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class GetGroupByCustomerId : IRequest<List<GroupDto>>
{
    /// <summary>
    /// The Id of the customer
    /// </summary>
    public Guid Id { get; set; }
}
public class GetGroupRequestHandler : IRequestHandler<GetGroupByCustomerId,List<GroupDto>>
{
    private readonly IRepository<Domain.Entities.Group, Guid> _repository;

    public GetGroupRequestHandler(IRepository<Domain.Entities.Group, Guid> repository)=>
        (_repository) = (repository);
    
    public async Task<List<GroupDto>> Handle(GetGroupByCustomerId request, CancellationToken cancellationToken)
    {
        var groups = await _repository.Table
            .Where(p => p.Followers.Any(x => x.Id == request.Id))
            .Include(p => p.Followers)
            .Include(p => p.Owner)
            .AsNoTracking()
            .Select(p => new GroupDto(p.Followers.Count,
                    new AccountDto(p.Owner.Name,p.Owner.Id),
                    p.Name,
                    p.Id,
                    p.AvatarPath)).ToListAsync(cancellationToken);
        return groups;
    }
}