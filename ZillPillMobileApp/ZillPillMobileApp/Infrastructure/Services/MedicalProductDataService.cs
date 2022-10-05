using Newtonsoft.Json;
using ZillPillMobileApp.Domain.DTO.MedicalPoduct;
using Microsoft.AspNetCore.WebUtilities;
using ZillPillMobileApp.MVVM.Model;
using System.Text;
using ZillPillMobileApp.Domain;
using System.Net.Http.Headers;
using ZillPillMobileApp.Domain.DTO.Shedullers;
using ZillPillMobileApp.Domain.Query;

namespace ZillPillMobileApp.Infrastructure.Services
{
    public class MedicalProductDataService
    {
        private readonly Uri _baseUrl = new("https://u0901873.plsk.regruhosting.ru/ZillPillService");
        // private readonly Uri _baseUrl = new("https://localhost:7243/");

        private readonly HttpClientHandler httpClientHandler = new();

        public MedicalProductDataService()
        {
            httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpClientHandler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true;
        }

        public async Task<List<MedicalPoductItemModel>> GetMedicalPoductListAsync(GetFilteredMedicalProductQuery query, int offset, int limit)
        {
            var client = new HttpClient(httpClientHandler);
            Dictionary<string, string> param = new()
            {
                { "offset", $"{offset}" },
                { "limit", $"{limit}" }
            };
            var urlToQuery = QueryHelpers.AddQueryString(_baseUrl.AbsoluteUri + "/api/medProd/list", param);
            var json = JsonConvert.SerializeObject(query);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await client.PostAsync(urlToQuery, data);

            var response = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new Exception(response);

            var responseDto = JsonConvert.DeserializeObject<List<MedicalProductDto>>(response);

            var result = new List<MedicalPoductItemModel>();
            foreach (var dto in responseDto)
                result.Add(new MedicalPoductItemModel(dto.ProductId, dto.ProductId, dto.Name, ImageSource.FromStream(() => new MemoryStream(dto.ImageData)), 0, 0, 0.0));

            return result;
        }

        public async Task<MedicalPoductDetailModel> GetMedicalPoductDetailAsync(int productId)
        {
            var client = new HttpClient(httpClientHandler);
            Dictionary<string, string> param = new()
            {
                { "productId", $"{productId}" }
            };
            var urlToQuery = QueryHelpers.AddQueryString(_baseUrl.AbsoluteUri + "/api/medProd/detail", param);
            var resp = await client.GetAsync(urlToQuery);

            var response = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new Exception(response);

            var responseDto = JsonConvert.DeserializeObject<MedicalPoductDetailDto>(response);

            return new MedicalPoductDetailModel()
            {
                Id = responseDto.Id,
                Name = responseDto.Name,
                Characteristics = responseDto.Characteristics,
                Chemicals = responseDto.Chemicals,
                Releases = responseDto.Releases,
                Certificate = new MedicalProductCertificateModel()
                {
                    RegisterDate = responseDto.Certificate.RegisterDate,
                    Approved = responseDto.Certificate.Approved,
                    License = responseDto.Certificate.License,
                },
                ImageSource = ImageSource.FromStream(() => new MemoryStream(responseDto.ImageData)),
            };
        }

        #region user

        public async Task<List<MedicalPoductItemModel>> GetMedicalPoductListForUserAsync()
        {
            List<MedicalPoductItemModel> resultT = new();

            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseHttp = await client.GetAsync(_baseUrl.AbsoluteUri + "/api/medProd/user/list");
            var response = await responseHttp.Content.ReadAsStringAsync();

            if (!responseHttp.IsSuccessStatusCode)
                throw new Exception(response);

            var responseDto = JsonConvert.DeserializeObject<List<UserMedicalProductDto>>(response);

            var result = new List<MedicalPoductItemModel>();
            foreach (var dto in responseDto)
                result.Add(new MedicalPoductItemModel(
                    dto.ProductId, dto.RelationId, dto.Name,
                    ImageSource.FromStream(() => new MemoryStream(dto.ImageData)),
                    dto.TotalToAccept, dto.TotalAccepted, dto.Progress));

            return result;
        }

        public async Task RemoveMedicalPoductListForUserAsync(int relationId)
        {
            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Dictionary<string, string> param = new()
            {
                { "relationId", $"{relationId}" }
            };
            var urlToQuery = QueryHelpers.AddQueryString(_baseUrl.AbsoluteUri + "/api/medProd/user/rm", param);
            var responseHttp = await client.DeleteAsync(urlToQuery);

            if (!responseHttp.IsSuccessStatusCode)
            {
                var response = await responseHttp.Content.ReadAsStringAsync();
                throw new Exception(response);
            }
        }

        public async Task AddMedProdToUser(int productId)
        {
            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Dictionary<string, string> param = new()
            {
                { "productId", $"{productId}" }
            };
            var urlToQuery = QueryHelpers.AddQueryString(_baseUrl.AbsoluteUri + "/api/medProd/user/add", param);
            var responseHttp = await client.GetAsync(urlToQuery);

            if (!responseHttp.IsSuccessStatusCode)
            {
                var response = await responseHttp.Content.ReadAsStringAsync();
                throw new Exception(response);
            }
        }

        #endregion user

        #region sheduller

        public async Task<SetShedullerToUserDto> GetMedProdShedullerForUserAsync(int relationId)
        {
            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Dictionary<string, string> param = new()
            {
                { "relationId", $"{relationId}" }
            };
            var urlToQuery = QueryHelpers.AddQueryString(_baseUrl.AbsoluteUri + "/api/medProd/user/sheduller", param);
            var responseHttp = await client.GetAsync(urlToQuery);

            var responseContent = await responseHttp.Content.ReadAsStringAsync();

            if (!responseHttp.IsSuccessStatusCode)
                throw new Exception(responseContent);

            var responseDto = JsonConvert.DeserializeObject<SetShedullerToUserDto>(responseContent);
            return responseDto;
        }

        public async Task SetMedProdShedullerForUserAsync(SetShedullerToUserDto query)
        {
            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var json = JsonConvert.SerializeObject(query);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var responseHttp = await client.PostAsync(_baseUrl.AbsoluteUri + $"/api/medProd/user/sheduller", data);

            if (!responseHttp.IsSuccessStatusCode)
            {
                var response = await responseHttp.Content.ReadAsStringAsync();
                throw new Exception(response);
            }
        }

        #endregion sheduller

        #region sheduller items

        public async Task<List<ShedullerItemDetailModel>> GetShedullerItemsByDayAsync(DateTime date)
        {
            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Dictionary<string, string> param = new()
            {
                { "date", $"{date:yyyy-MM-ddTHH:mm:ss}" }
            };

            var urlToQuery = QueryHelpers.AddQueryString(_baseUrl.AbsoluteUri + "/api/medProd/user/sheduller/item", param);
            var responseHttp = await client.GetAsync(urlToQuery);

            var responseContent = await responseHttp.Content.ReadAsStringAsync();

            if (!responseHttp.IsSuccessStatusCode)
                throw new Exception(responseContent);

            var responseDto = JsonConvert.DeserializeObject<List<ShedullerItemDetailDto>>(responseContent);

            List<ShedullerItemDetailModel> result = new();
            responseDto.ForEach(x =>
            {
                var sp = x.Time.ToString().Split(":");
                var timeStr = $"{sp[0]}:{sp[1]}";
                result.Add(new ShedullerItemDetailModel()
                {
                    Date = x.Date,
                    IsAccepted = x.IsAccepted,
                    IsSended = x.IsSended,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    ShedullerItemId = x.ShedullerItemId,
                    Time = timeStr,
                    ImageSource = ImageSource.FromStream(() => new MemoryStream(x.ImageData))
                });
            });

            return result;
        }

        public async Task AcceptShedullerItemAsync(int shedullerItemId)
        {
            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Dictionary<string, string> param = new()
            {
                { "shedullerItemId", $"{shedullerItemId}" }
            };

            var urlToQuery = QueryHelpers.AddQueryString(_baseUrl.AbsoluteUri + "/api/medProd/user/sheduller/accept", param);
            var responseHttp = await client.GetAsync(urlToQuery);

            if (!responseHttp.IsSuccessStatusCode)
            {
                var response = await responseHttp.Content.ReadAsStringAsync();
                throw new Exception(response);
            }
        }

        #endregion sheduller items
    }
}
