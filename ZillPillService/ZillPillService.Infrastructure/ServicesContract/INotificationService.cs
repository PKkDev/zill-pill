using ZillPillService.Domain.Query.Notification;

namespace ZillPillService.Infrastructure.ServicesContract
{
    public interface INotificationService
    {
        public Task SendAll(string topick, NotificationQuery message);
    }
}
