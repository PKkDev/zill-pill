using FirebaseAdmin.Messaging;
using ZillPillService.Domain.Model;
using ZillPillService.Domain.Query.Notification;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.Application.Services
{
    public class NotificationService : INotificationService
    {
        public async Task SendNotifAsync(string topick, NotificationQuery message, CancellationToken ct)
        {
            // await SendToTopick(message.Type, message.Title, message.Describle);
            await SendToTopick(topick, message.Title, message.Describle, ct);
        }

        private async Task SendToTopick(NotificationTypeEnum type, string title, string Describle, CancellationToken ct)
        {
            string topic = String.Empty;
            switch (type)
            {
                case NotificationTypeEnum.System:
                    topic = "system";
                    break;
                case NotificationTypeEnum.Sheduller:
                    topic = "sheduller";
                    break;
            }

            Message message = new()
            {
                Topic = topic,
                Data = new Dictionary<string, string>()
                {
                    { "title", title },
                    { "body", Describle },
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                }
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message, ct);
        }

        private async Task SendToTopick(string topic, string title, string Describle, CancellationToken ct)
        {
            Message message = new()
            {
                Topic = topic,
                Data = new Dictionary<string, string>()
                {
                    { "title", title },
                    { "body", Describle },
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                }
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message, ct);
        }
    }
}
