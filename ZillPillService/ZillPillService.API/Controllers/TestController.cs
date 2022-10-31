using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Parser.ConsoleApp;
using System.Text;
using ZillPillService.Domain.Exceptions;
using ZillPillService.Infrastructure.Context;
using ZillPillService.Infrastructure.Entities;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.API.Controllers
{
    public class GoogleSearchResult
    {
        [JsonProperty("images_results")]
        public List<ImagesResults> ImagesResults { get; set; }
    }
    public class ImagesResults
    {
        [JsonProperty("position")]
        public string position { get; set; }

        [JsonProperty("thumbnail")]
        public string thumbnail { get; set; }

        [JsonProperty("source")]
        public string source { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("link")]
        public string link { get; set; }

        [JsonProperty("original")]
        public string original { get; set; }

        [JsonProperty("is_product")]
        public string is_product { get; set; }
    }

    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDataBaseContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICheckShedullerService _checkShedullerService;

        public TestController(AppDataBaseContext context, IHttpClientFactory httpClientFactory, ICheckShedullerService checkShedullerService)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _checkShedullerService = checkShedullerService;
        }

        [HttpGet("images")]
        public async Task SaveImages(CancellationToken ct = default)
        {
            throw new ApiException("not supported");

            var countries = await _context.CountryDictionary.ToListAsync(ct);

            var directory = "D:/work/labs/Zelenko/images_flag/75x50";
            DirectoryInfo di = new(directory);
            var files = di.GetFiles();
            foreach (var country in countries)
            {
                var found = files.FirstOrDefault(x => x.Name.Contains(country.Name));
                if (found != null)
                {
                    byte[] readText = System.IO.File.ReadAllBytes(found.FullName);
                    country.ImageData = readText;

                    _context.CountryDictionary.Update(country);
                    await _context.SaveChangesAsync(ct);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> DoTest(CancellationToken ct = default)
        {
            throw new ApiException("not supported");

            var parseResult = ParserWorker.Parse();
            var result = parseResult.Take(100).ToList();
            result = result.Where(x => x.ManufacturerCountries.Any()).ToList();

            var totalCountries = result.SelectMany(x => x.ManufacturerCountries).Distinct().ToList();
            foreach (var country in totalCountries)
            {
                if (_context.CountryDictionary.Any(x => x.Name.Equals(country)))
                    continue;
                var newCountry = new CountryDictionary()
                {
                    ImageData = null,
                    Name = country,
                };
                await _context.CountryDictionary.AddAsync(newCountry);
                await _context.SaveChangesAsync(ct);
            }

            var client = _httpClientFactory.CreateClient();
            List<MedicinalProduct> preparats = new();
            foreach (var item in result)
            {
                MedicinalProduct newProduct = new()
                {
                    Name = item.TradeName,
                    Manufacturer = item.Manufacturer,
                    Characteristics = item.Characteristics,
                    Certificate = new MedicinalProductCertificate()
                    {
                        Approved = item.RegistrationCertificate.Approved,
                        RegisterDate = item.RegistrationCertificate.RegisterDate,
                        License = item.RegistrationCertificate.License
                    },
                    Chemicals = item.ChemicalName.Select(x => new MedicinalProductChemical() { Name = x }).ToList(),
                    Releases = item.ReleaseForm.Select(x => new MedicinalProductRelease() { Name = x }).ToList(),
                    Image = new MedicinalProductImage(),
                    Country = _context.CountryDictionary.First(x => x.Name.Equals(item.ManufacturerCountries.First()))
                };


                try
                {
                    String apiKey = "5a3258fe887c4d69b4ec5816d4a0ac1cc7248e2d87ad88856d0d615296d1c6c6";
                    var uri = "https://serpapi.com/search.json";
                    Dictionary<string, string> param = new()
                    {
                        { "tbm","isch" },
                        { "ijn","0" },
                        { "api_key",apiKey }
                    };
                    var uriParsed = QueryHelpers.AddQueryString(uri, param);
                    uriParsed += $"&q={item.TradeName}&tbs=isz:i";
                    //uriParsed += $"&q=Apple&tbs=isz:i";
                    var response = await client.GetAsync(uriParsed, ct);
                    var text = await response.Content.ReadAsStringAsync(ct);
                    var res = JsonConvert.DeserializeObject<GoogleSearchResult>(text);

                    var firstImage = res.ImagesResults.First();

                    var query =
                        (firstImage.original.Contains(".png") || firstImage.original.Contains(".jpeg") || firstImage.original.Contains(".jpg"))
                        ? firstImage.original
                        : (firstImage.source.Contains(".png") || firstImage.source.Contains(".jpeg") || firstImage.source.Contains(".jpg"))
                        ? firstImage.source
                        : (firstImage.link.Contains(".png") || firstImage.link.Contains(".jpeg") || firstImage.link.Contains(".jpg"))
                        ? firstImage.link
                        : null;

                    var resp2 = await client.GetAsync(firstImage.original);
                    var parh = $"D:/work/labs/Zelenko/images/{item.TradeName}.png";
                    using var sw = new StreamWriter(parh, false);
                    var f = resp2.Content.CopyToAsync(sw.BaseStream);
                    var bytes = await resp2.Content.ReadAsByteArrayAsync();

                    // var bytes = System.IO.File.ReadAllBytes(parh);
                    newProduct.Image.Data = bytes;
                }
                catch (Exception e)
                {
                    newProduct.Image.Data = null;
                }

                preparats.Add(newProduct);
            }

            await _context.MedicinalProduct.AddRangeAsync(preparats, ct);
            await _context.SaveChangesAsync(ct);

            var m = await _context.MedicinalProduct
                .Include(x => x.Image)
                .Include(x => x.Chemicals)
                .Include(x => x.Releases)
                .Include(x => x.Certificate)
                .ToListAsync();

            var img = m.First().Image.Data;
            return base.File(img, "image/jpeg");

            //var user = new User()
            //{
            //    Phone = "89372174165",
            //    Code = "4234",
            //    Profile = new UserProfile()
            //    {
            //        Email = "email",
            //        FirstName = "name",
            //    }
            //};

            //await _context.User.AddAsync(user);
            //await _context.SaveChangesAsync();

            // var g = await _context.User.ToListAsync();
            return Ok();
        }

    }
}
