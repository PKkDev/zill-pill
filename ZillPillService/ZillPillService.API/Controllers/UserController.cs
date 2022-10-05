using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZillPillService.Domain.DTO.User;
using ZillPillService.Domain.Query.User;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// send sms with code to user
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("send-sms")]
        public async Task SendAccesTokenToSms(
            [FromBody] PhoneAuthorizeQuery query, CancellationToken ct = default)
        {
            await _service.SendAccesTokenToSmsAsync(query.Phone, ct);
        }

        /// <summary>
        /// check code from sms and user
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("check-sms")]
        public async Task<LoginResponseDto> CheckPhoneAccessToken(
            [FromBody] CheckPhoneAuthorizeQuery query, CancellationToken ct = default)
        {
            return await _service.CheckPhoneAccessTokenAsync(query.Phone, query.Code, ct);
        }

        /// <summary>
        /// test check auth
        /// </summary>
        /// <returns></returns>
        [HttpGet("check-auth")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CheckAuth() => Ok("all ok!");


        /// <summary>
        /// get user detail
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("detail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<UserDetailDto> GetUserDetail(CancellationToken ct = default)
        {
            var userId = HttpContext.GetUserId();
            return await _service.GetUserDetailAsync(userId, ct);
        }

        /// <summary>
        /// update user detail
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("detail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task UpdateUserDetail([FromBody] UpdateUserDetailQuery query, CancellationToken ct = default)
        {
            var userId = HttpContext.GetUserId();
            await _service.UpdateUserDetailAsync(userId, query, ct);
        }

        /// <summary>
        /// delete user
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task DeleteUser(CancellationToken ct = default)
        {
            var userId = HttpContext.GetUserId();
            await _service.DeleteUserAsync(userId, ct);
        }
    }
}
