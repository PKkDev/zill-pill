using Microsoft.AspNetCore.Mvc;
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
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("check")]
        // [AllowAnonymous]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [FromBody] NotificationQuery message
        public async Task CheckShedullers(CancellationToken ct = default)
            => await _checkShedullerService.CheckShedullersAsync(ct);


        /// <summary>
        /// отправка системного сообщения
        /// </summary>
        /// <param name="body"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("sys-mes")]
        public async Task SendSystemMessge([FromQuery] string body, CancellationToken ct = default)
            => await _checkShedullerService.SendSystemMessgeAsync(body, ct);


    }
}
