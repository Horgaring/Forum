using BuildingBlocks.Core.Repository;
using Domain.Entities;

namespace Infrastructure.Context;

public class CustomerIdRepository: Repository<CustomerId,Guid,PostDbContext>
{
    public CustomerIdRepository(PostDbContext db) : base(db)
    {
    }
}