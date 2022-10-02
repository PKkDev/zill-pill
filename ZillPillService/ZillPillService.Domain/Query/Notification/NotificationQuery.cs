using ZillPillService.Domain.Model;

namespace ZillPillService.Domain.Query.Notification
{
    public class NotificationQuery
    {
        public string Title { get; set; }

        public string Describle { get; set; }

        public NotificationTypeEnum Type { get; set; }
    }
}
