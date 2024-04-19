using BuildingBlocks.Core.Model;

namespace BuildingBlocks;

public class OutBoxMessage : Entity<Guid>
{
    public OutBoxMessage(){}

    public OutBoxMessage(DateTime occurredOnUtc, string? type, string data)
    {
        OccurredOnUtc = occurredOnUtc;
        Type = type;
        Data = data;
        
    }
    public DateTime OccurredOnUtc { get; set; }

    public string? Type { get; set; }


    public string Data { get; set; }

}
