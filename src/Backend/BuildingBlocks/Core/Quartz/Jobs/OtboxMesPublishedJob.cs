using System.Text.Json;
using BuildingBlocks.Core.Model;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using Serilog;

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
            
            var domainevent = JsonConvert.DeserializeObject(outBoxMessage.Data, new JsonSerializerSettings{
                TypeNameHandling = TypeNameHandling.Auto
            });
            if (domainevent == null)
            {
                continue;
            }
            await _publish.Publish(domainevent);
        }
        if (outBoxMessages.Count > 0) Log.Information($"Published {outBoxMessages.Count} events to outbox");
        _db.Set<OutBoxMessage>().RemoveRange(outBoxMessages);
        await _db.SaveChangesAsync();
    }
}
