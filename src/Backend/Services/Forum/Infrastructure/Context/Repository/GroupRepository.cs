using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context.Repository;

public class GroupRepository : Repository<Group, Guid, PostDbContext>
{
    public GroupRepository(PostDbContext db) : base(db)
    {
    }



    public override Task<Group> CreateAsync(Group entity)
    {


        return base.CreateAsync(entity);
    }
}