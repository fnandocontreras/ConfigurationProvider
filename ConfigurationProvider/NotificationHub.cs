using Microsoft.AspNetCore.SignalR;

namespace ConfigurationProvider
{
    public interface INotificationHub
    {
        Task ConfigurationChanged(string message);
    }

    public class NotificationHub: Hub<INotificationHub>
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var groupName = httpContext.Request.Query["appName"].FirstOrDefault();
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await base.OnConnectedAsync();
        }
    }
}
