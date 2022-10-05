using Microsoft.EntityFrameworkCore;
using ZillPillService.Domain.DTO.Countries;
using ZillPillService.Infrastructure.Context;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.Application.Services
{
    public class CountriesService : ICountriesService
    {
        private readonly AppDataBaseContext _context;

        public CountriesService(AppDataBaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CountryDetailDto>> GetCountriesAsync(CancellationToken ct)
        {
            var result = await _context.CountryDictionary
                .Select(x => new CountryDetailDto(x.Id, x.Name, x.ImageData))
                .ToListAsync(ct);
            return result;
        }
    }
}
