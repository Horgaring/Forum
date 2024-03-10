using BuildingBlocks.Core.Repository;
using Domain.Entities;

namespace Infrastructure.Context;

public class GroupRepository : Repository<Group,Guid,PostDbContext>
{
    public GroupRepository(PostDbContext db) : base(db)
    {
    }

    public override Task<Group?> GetByIdAsync(Guid id)
    {
        return base.GetByIdAsync(id);
    }

    public override Task<Group> CreateAsync(Group entity)
    {
        if (Db.CustomersId.Find(entity.Followers.First()) == null)
        {
            Db.CustomersId.Add(entity.Followers.First());
        }
        return base.CreateAsync(entity);
    }
}