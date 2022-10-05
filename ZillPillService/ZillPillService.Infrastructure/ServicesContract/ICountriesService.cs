using ZillPillService.Domain.DTO.Countries;

namespace ZillPillService.Infrastructure.ServicesContract
{
    public interface ICountriesService
    {
        public Task<IEnumerable<CountryDetailDto>> GetCountriesAsync(CancellationToken ct);
    }
}
