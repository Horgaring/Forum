using System.Text.Json;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Quartz;

namespace BuildingBlocks;

public class OtboxMesPublishedJob<TContext> : IJob
where TContext : DbContext
{ 
    public static JobKey JobKey { get; set; } = new JobKey("OtboxMesPublishedJob");

    private TContext _db { get; set; }
    public IPublishEndpoint _publish { get; set; }

    public OtboxMesPublishedJob(TContext db, IPublishEndpoint publish)
    {
        _db = db;
        _publish = publish;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var outBoxMessages = await _db.Set<OutBoxMessage>()
        .Take(30)
        .ToListAsync();

        foreach (var outBoxMessage in outBoxMessages)
        {
            var domainevent = JsonSerializer.Deserialize<DomainEvent>(outBoxMessage.Data);
            if (domainevent == null)
            {
                continue;
            }
            await _publish.Publish(domainevent);
        }
;
        _db.Set<OutBoxMessage>().RemoveRange(outBoxMessages);
    }
}
