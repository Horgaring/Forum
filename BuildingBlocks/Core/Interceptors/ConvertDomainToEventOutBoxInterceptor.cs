using System.Text.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;

namespace BuildingBlocks;

public class ConvertDomainToEventOutBoxInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var outBoxMessages = eventData.Context.ChangeTracker
        .Entries<AggregateRoot<Guid>>()
        .Select(p => p.Entity)
        .SelectMany(p => 
        {
            var events = p.GetDomainEvents();
            p.ClearEvents();
            return events;
        })
        .Select(p => new OutBoxMessage(
            DateTime.UtcNow,
            p.GetType().Name,
            JsonSerializer.Serialize(p)
        ))
        .ToList();
        
        eventData.Context.Set<OutBoxMessage>().AddRange(outBoxMessages);
        Log.Information($"Added {outBoxMessages.Count} events to outbox");
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
