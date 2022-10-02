using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZillPillService.Domain.Query.Notification;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.API.Controllers
{
    [Route("api/Notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// отправка системного сообщения
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost("send")]
        // [AllowAnonymous]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task PushSystemMessage([FromBody] NotificationQuery message)
        {
            await _notificationService.SendAll(message);
        }

    }
}
