using BuildingBlocks.Core.Model;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class DayActivity : Entity<Guid>
{
    public DateOnly Date { get; set; }
    public int Activity { get; set; }
    
    public string UserId { get; set; }
    public User User { get; set; }

    public static async Task CreateOrUpdatetoDb(DbContext context)
    {
        if (context.Set<DayActivity>().SingleOrDefault(e => e.Date == DateOnly.MaxValue) is DayActivity active)
        {
            active.Activity += 1;
            
        }
        else
        {
            await context.Set<DayActivity>().AddAsync(new DayActivity() { Activity = 1, Date = DateOnly.MaxValue });
        }

        await context.SaveChangesAsync();
    }
}