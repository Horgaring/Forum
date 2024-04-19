using BuildingBlocks.Core.Events.Post;
using Domain;
using Infrastructure.Context;
using MassTransit;

namespace Infrastructure.Consumers;

public class DeletedPostConsumer : IConsumer<CreatedPostEvent>
{
    private ApplicationDbContext _context;

    public DeletedPostConsumer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<CreatedPostEvent> context)
    {
        DayActivity.CreateOrUpdatetoDb(_context);
    }
}