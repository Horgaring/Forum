using BuildingBlocks.Core.Events;
using BuildingBlocks.Core.Events.Post;
using BuildingBlocks.Core.Repository;
using Infrastructure.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Bus;

public class PostDeletedConsumer: IConsumer<DeletedPostEvent>
{
    private readonly CommentRepository _db;
    
    private readonly IUnitOfWork<CommentDbContext> _uow;

    public PostDeletedConsumer(CommentRepository db, IUnitOfWork<CommentDbContext> uow)
    {
        _db = db;
        _uow = uow;
    }

    public async Task Consume(ConsumeContext<DeletedPostEvent> context)
    {
        var taregetComment = await _db.Where(p => p.Postid == context.Message.PostId).ExecuteDeleteAsync();
        await _uow.CommitAsync();
    }
}