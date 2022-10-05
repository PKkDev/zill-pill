using FirebaseAdmin.Messaging;
using ZillPillService.Domain.Model;
using ZillPillService.Domain.Query.Notification;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.Application.Services
{
    public class NotificationService : INotificationService
    {
        public async Task SendAll(string topick, NotificationQuery message)
        {
            // await SendToTopick(message.Type, message.Title, message.Describle);
            await SendToTopick(topick, message.Title, message.Describle);
        }

        private async Task SendToTopick(NotificationTypeEnum type, string Title, string Describle)
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
                    { "title", Title },
                    { "body", Describle },
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                }
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        private async Task SendToTopick(string topic, string Title, string Describle)
        {
            Message message = new()
            {
                Topic = topic,
                Data = new Dictionary<string, string>()
                {
                    { "title", Title },
                    { "body", Describle },
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                }
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
