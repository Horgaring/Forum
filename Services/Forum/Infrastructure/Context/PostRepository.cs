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

   
    
    public async Task<Post?> SingleOrDefaultAsync(Expression<Func<Post?, bool>> func)
    {
        return await Db.Post.SingleOrDefaultAsync(func);
    }
}