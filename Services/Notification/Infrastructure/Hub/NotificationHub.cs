using IdentityModel;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Hub;

[Authorize]
public class NotificationsHub : Microsoft.AspNetCore.SignalR.Hub<INotificationsClient>
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Claims
            .First(op => op.Type ==  JwtClaimTypes.Subject).Value);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception ex)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Claims
            .First(op => op.Type ==  JwtClaimTypes.Subject).Value);
        await base.OnDisconnectedAsync(ex);
    }
}

public interface INotificationsClient
{
    public void Notify(string message);
}