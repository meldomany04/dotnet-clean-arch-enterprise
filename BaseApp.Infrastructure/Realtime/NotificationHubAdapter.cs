using BaseApp.Application.Common.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace BaseApp.Infrastructure.Realtime
{
    public class NotificationHubAdapter : INotificationHub
    {
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationHubAdapter(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public Task NotifyAllAsync(string eventName, object payload)
        {
            return _hub.Clients.All.SendAsync(eventName, payload);
        }

        public Task NotifyUserAsync(string userId, string eventName, object payload)
        {
            return _hub.Clients.User(userId).SendAsync(eventName, payload);
        }
    }
}
