using System.Linq.Expressions;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class PostRepository : Repository<Post,Guid,PostDbContext>
{
    public PostRepository(PostDbContext db) : base(db)
    {
    }

    public override Task<Post> CreateAsync(Post entity)
    {
        if (Db.CustomersId.Find(entity.Userid) == null)
        {
            Db.CustomersId.Add(entity.Userid);
        }
        return base.CreateAsync(entity);
    }
    
    public async Task<Post?> SingleOrDefaultAsync(Expression<Func<Post?, bool>> func)
    {
        return await Db.Post.SingleOrDefaultAsync(func);
    }
}