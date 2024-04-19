using BuildingBlocks.Core.Events.Post;
using Domain;
using Infrastructure.Context;
using MassTransit;

namespace Infrastructure.Consumers;

public class CreatedPostConsumer : IConsumer<CreatedPostEvent>
{
    private ApplicationDbContext _context;

    public CreatedPostConsumer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<CreatedPostEvent> context)
    {
        DayActivity.CreateOrUpdatetoDb(_context);
    }
}