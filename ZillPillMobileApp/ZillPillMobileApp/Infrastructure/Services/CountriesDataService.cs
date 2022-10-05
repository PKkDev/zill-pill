using Newtonsoft.Json;
using System.Net.Http.Headers;
using ZillPillMobileApp.Domain.DTO.Countries;
using ZillPillMobileApp.MVVM.Model;

namespace ZillPillMobileApp.Infrastructure.Services
{
    public class CountriesDataService
    {
        private readonly Uri _baseUrl = new("https://u0901873.plsk.regruhosting.ru/ZillPillService");
        // private readonly Uri _baseUrl = new("https://localhost:7243/");

        private readonly HttpClientHandler httpClientHandler = new();

        public CountriesDataService()
        {
            httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpClientHandler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true;
        }

        public async Task<List<CountryDetailModel>> GetCountriesAsync()
        {
            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseHttp = await client.GetAsync(_baseUrl.AbsoluteUri + "/api/countries/list");
            var response = await responseHttp.Content.ReadAsStringAsync();

            if (!responseHttp.IsSuccessStatusCode)
                throw new Exception(response);

            var responseDto = JsonConvert.DeserializeObject<List<CountryDetailDto>>(response);

            var result = new List<CountryDetailModel>();

            foreach (var dto in responseDto)
                result.Add(new CountryDetailModel(dto.CountryId, dto.Name, ImageSource.FromStream(() => new MemoryStream(dto.ImageData))));

            return result;
        }
    }
}
