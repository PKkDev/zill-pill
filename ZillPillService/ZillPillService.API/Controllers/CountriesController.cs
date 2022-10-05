using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZillPillService.Domain.DTO.Countries;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.API.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesService _service;

        public CountriesController(ICountriesService service)
        {
            _service = service;
        }

        [HttpGet("list")]
        public async Task<IEnumerable<CountryDetailDto>> GetCountries(CancellationToken ct)
        {
            return await _service.GetCountriesAsync(ct);
        }
    }
}
