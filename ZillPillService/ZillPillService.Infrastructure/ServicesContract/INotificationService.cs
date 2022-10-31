using ZillPillService.Domain.Query.Notification;

namespace ZillPillService.Infrastructure.ServicesContract
{
    public interface INotificationService
    {
        public Task SendNotifAsync(string topick, NotificationQuery message, CancellationToken ct);
    }
}
