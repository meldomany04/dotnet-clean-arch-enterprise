namespace BaseApp.Application.Common.Realtime
{
    public interface INotificationHub
    {
        Task NotifyAllAsync(string eventName, object payload);
        Task NotifyUserAsync(string userId, string eventName, object payload);
    }

}