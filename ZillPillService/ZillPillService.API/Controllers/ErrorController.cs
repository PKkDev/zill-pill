using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZillPillService.Application.Services;
using ZillPillService.Domain.DTO.Error;
using ZillPillService.Domain.Exceptions;

namespace ZillPillService.API.Controllers
{
    /// <summary>
    /// accept service error on prod
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ErrorNotificationService _serice;

        public ErrorController(ErrorNotificationService serice)
        {
            _serice = serice;
        }

        /// <summary>
        /// generate error response from service
        /// </summary>
        /// <returns></returns>
        [Route("error")]
        public async Task<HttpErrorMessage> AcceptAPIError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;

            await _serice.SendNotifToTelegram(exception.Message);

            var code = 500;
            // var message = exception.Message;
            var message = "Exception occurred";

            if (exception is ApiException httpException)
            {
                code = (int)httpException.Code;
                message = httpException.Error;
            }

            Response.StatusCode = code;
            var response = new HttpErrorMessage(message);
            return response;
        }
    }
}
