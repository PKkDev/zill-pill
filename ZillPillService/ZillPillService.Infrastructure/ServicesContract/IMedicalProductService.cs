using ZillPillService.Domain.DTO.MedicalProduct;
using ZillPillService.Domain.DTO.Shedullers;
using ZillPillService.Domain.Query;
using ZillPillService.Domain.Query.User;

namespace ZillPillService.Infrastructure.ServicesContract
{
    public interface IMedicalProductService
    {
        public Task<List<MedicalProductDto>> GetMedicalProductsListAsync(GetFilteredMedicalProductQuery filter, int offset, int limit, CancellationToken ct);

        public Task<MedicalProductDetailDto> GetMedicalProductsDetailAsync(int productId, CancellationToken ct);

        #region user

        public Task<List<UserMedicalProductDto>> GetMedProdForUserAsync(int userId, CancellationToken ct);

        public Task AddMedProdToUserAsync(int userId, int productId, CancellationToken ct);

        public Task RemoveMedProdFromUserAsync(int relationId, CancellationToken ct);

        #endregion user

        #region sheduller

        public Task<SetShedullerToUserDto> GetMedProdShedullerForUserAsync(int relationId, CancellationToken ct);

        public Task SetMedProdShedullerForUserAsync(SetShedullerToUserDto query, CancellationToken ct);

        #endregion sheduller

        #region sheduller items

        public Task<IEnumerable<ShedullerItemDetailDto>> GetShedullerItemsByDayAsync(int userId, DateTime date, CancellationToken ct);

        public Task AcceptShedullerItemAsync(int shedullerItemId, CancellationToken ct);

        #endregion sheduller items
    }
}
