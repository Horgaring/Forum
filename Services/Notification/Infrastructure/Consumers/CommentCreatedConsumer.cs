using BuildingBlocks.Events;
using BuildingBlocks.Events.Comment;
using Infrastructure.Hub;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Consumers;

public class CommentCreatedConsumer: IConsumer<CommentCreatedEvent>
{
    private readonly IHubContext<NotificationsHub,INotificationsClient> _hub;
    private const string _format = """
                                   User {0} left a comment: “{1}”
                                   """;

    public CommentCreatedConsumer(IHubContext<NotificationsHub,INotificationsClient> hub)
    {
        _hub = hub;
    }

    public async Task Consume(ConsumeContext<CommentCreatedEvent> context)
    {
        _hub.Clients
            .Group(context.Message.CustomerId)
            .Notify(string.Format(_format,context.Message.CustomerName,context.Message.Content));
    }
}