using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZillPillService.Application.Services;
using ZillPillService.Domain.Query.Notification;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.API.Controllers
{
    [Route("api/Notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ICheckShedullerService _checkShedullerService;

        public NotificationsController(ICheckShedullerService checkSheduller)
        {
            _checkShedullerService = checkSheduller;
        }

        /// <summary>
        /// отправка системного сообщения
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet("check")]
        // [AllowAnonymous]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [FromBody] NotificationQuery message
        public async Task PushSystemMessage()
        {
            await _checkShedullerService.CheckSheduller();
        }

    }
}
