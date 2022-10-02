using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Parser.ConsoleApp;
using System.Text;
using ZillPillService.Infrastructure.Context;
using ZillPillService.Infrastructure.Entities;


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
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDataBaseContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public TestController(AppDataBaseContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> DoTest(CancellationToken ct = default)
        {
            var result1 = ParserWorker.Parse();
            var result = result1.Take(90).ToList();

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
                    Image = new MedicinalProductImage()
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
                    uriParsed += $"&q=Apple&tbs=isz:i";
                    var response = await client.GetAsync(uriParsed, ct);
                    var text = await response.Content.ReadAsStringAsync(ct);
                    var res = JsonConvert.DeserializeObject<GoogleSearchResult>(text);

                    var firstImage = res.ImagesResults.First();

                    var resp2 = await client.GetAsync(firstImage.original);
                    var parh = "D:/work/labs/Zelenko/ZillPillService/image.png";
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
