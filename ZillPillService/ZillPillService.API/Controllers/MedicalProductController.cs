using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZillPillService.Domain.DTO.MedicalProduct;
using ZillPillService.Domain.DTO.Shedullers;
using ZillPillService.Domain.Query;
using ZillPillService.Domain.Query.User;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.API.Controllers
{
    [Route("api/medProd")]
    [ApiController]
    public class MedicalProductController : ControllerBase
    {
        private readonly IMedicalProductService _service;

        public MedicalProductController(IMedicalProductService service)
        {
            _service = service;
        }

        [HttpPost("list")]
        public async Task<IEnumerable<MedicalProductDto>> GetMedicalProductsList(
          [FromBody] GetFilteredMedicalProductQuery query, [FromQuery] int offset, [FromQuery] int limit, CancellationToken ct = default)
        {
            return await _service.GetMedicalProductsListAsync(query, offset, limit, ct);
        }

        [HttpGet("detail")]
        public async Task<MedicalProductDetailDto> GetMedicalProductsDetail(
            [FromQuery] int productId, CancellationToken ct = default)
        {
            return await _service.GetMedicalProductsDetailAsync(productId, ct);
        }

        #region user

        [HttpGet("user/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<UserMedicalProductDto>> GetUserMedProd(
            CancellationToken ct = default)
        {
            var userId = HttpContext.GetUserId();
            return await _service.GetMedProdForUserAsync(userId, ct);
        }

        [HttpGet("user/add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task AddMedProdToUser(
            [FromQuery] int productId, CancellationToken ct = default)
        {
            var userId = HttpContext.GetUserId();
            await _service.AddMedProdToUserAsync(userId, productId, ct);
        }

        [HttpDelete("user/rm")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task RemoveMedProdFromUser(
            [FromQuery] int relationId, CancellationToken ct = default)
        {
            await _service.RemoveMedProdFromUserAsync(relationId, ct);
        }

        #endregion user

        #region sheduller

        [HttpGet("user/sheduller")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<SetShedullerToUserDto> GetMedProdShedullerForUser(
            [FromQuery] int relationId, CancellationToken ct = default)
        {
            return await _service.GetMedProdShedullerForUserAsync(relationId, ct);
        }

        [HttpPost("user/sheduller")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task SetMedProdShedullerForUser(
            [FromBody] SetShedullerToUserDto query, CancellationToken ct = default)
        {
            await _service.SetMedProdShedullerForUserAsync(query, ct);
        }

        #endregion sheduller

        #region sheduller items

        [HttpGet("user/sheduller/item")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<ShedullerItemDetailDto>> GetShedullerItemsByDay(
            [FromQuery] DateTime date, CancellationToken ct = default)
        {
            var userId = HttpContext.GetUserId();
            return await _service.GetShedullerItemsByDayAsync(userId, date, ct);
        }

        [HttpGet("user/sheduller/accept")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task AcceptShedullerItem(
            [FromQuery] int shedullerItemId, CancellationToken ct = default)
        {
            await _service.AcceptShedullerItemAsync(shedullerItemId, ct);
        }

        #endregion sheduller items
    }
}
