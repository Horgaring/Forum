using Application.Exceptions.Common;
using Application.Exceptions.Group;
using BuildingBlocks.Core.Events.Post;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Context.Repository;
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
    

    
    private readonly IUnitOfWork<PostDbContext> _uow;

    public LeaveFromGroupRequestHandler(GroupRepository repository, IUnitOfWork<PostDbContext> uow)
    {
        (_repository) = (repository);
        _uow = uow;
        
    }

    public async Task Handle(LeaveFromGroupRequest request, CancellationToken cancellationToken)
    {
        var group =  _repository.Table.Include(p => p.Followers)
            .Include(p => p.Posts)
            .FirstOrDefault(p => p.Name == request.Name) ?? throw new GroupNotFoundExeption();

        group.Followers.RemoveAll(p => p.Id == request.CustomerId);

        if (group.Followers.Count == 0)
        {
            for(int i = 0; i < group.Posts.Count; i++)
            {
                group.Posts[i].RaiseEvent(new DeletedPostEvent(){
                    PostId = group.Posts[i].Id
                });
            }
        }
        
        await _uow.CommitAsync();
    }
}