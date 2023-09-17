using BuildingBlocks.Events;
using Infrastructure.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Bus;

public class PostDeletedConsumer: IConsumer<DeletedPostEvent>
{
    private readonly CommentDbContext _db;

    public PostDeletedConsumer(CommentDbContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<DeletedPostEvent> context)
    {
        var taregetComment = await _db.Comment.SingleOrDefaultAsync(comment => comment.Postid == context.Message.PostId);
        if (taregetComment == null)
        {
            return;
        }
        _db.Comment.Remove(taregetComment);
    }
}