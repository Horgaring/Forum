using System.Linq.Expressions;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class CommentRepository : Repository<Comment,Guid,CommentDbContext>
{
    public CommentRepository(CommentDbContext db) : base(db)
    {
    }

    public async Task<Comment> SingleOrDefaultAsync(Expression<Func<Comment, bool>> func)
    {
        return await _db.Comment.SingleOrDefaultAsync(func);
    }
}