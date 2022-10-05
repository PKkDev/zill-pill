using Newtonsoft.Json;
using System.Text;
using ZillPillMobileApp.Domain.DTO.User;
using ZillPillMobileApp.Domain.Query.User;
using ZillPillMobileApp.MVVM.Model;

namespace ZillPillMobileApp.Infrastructure.Services
{
    public class UserDataService
    {
        private readonly Uri _baseUrl = new("https://u0901873.plsk.regruhosting.ru/ZillPillService");
        // private readonly Uri _baseUrl = new("https://localhost:7243/");

        private readonly HttpClientHandler httpClientHandler = new();

        public UserDataService()
        {
            httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpClientHandler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true;
        }

        public async Task GetCodeForUserAsync(string phone)
        {
            var client = new HttpClient(httpClientHandler);
            var query = new PhoneAuthorizeQuery(phone);
            var json = JsonConvert.SerializeObject(query);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await client.PostAsync(_baseUrl.AbsoluteUri + "/api/user/send-sms", data);

            if (!resp.IsSuccessStatusCode)
            {
                var response = await resp.Content.ReadAsStringAsync();
                throw new Exception(response);
            }
        }

        public async Task<LoginResponseDto> CheckCodeForUserAsync(string phone, string code)
        {
            var client = new HttpClient(httpClientHandler);
            var query = new CheckPhoneAuthorizeQuery(phone, code);
            var json = JsonConvert.SerializeObject(query);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await client.PostAsync(_baseUrl.AbsoluteUri + "/api/user/check-sms", data);
            var response = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new Exception(response);

            return JsonConvert.DeserializeObject<LoginResponseDto>(response);
        }

        public async Task<UserDetailDtoModel> GetUserDetailAsync()
        {
            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var resp = await client.GetAsync(_baseUrl.AbsoluteUri + "/api/user/detail");

            var response = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new Exception(response);

            var dto = JsonConvert.DeserializeObject<UserDetailDto>(response);
            return new(dto.Phone, dto.Email, dto.FirstName);
        }

        public async Task UpdateUserDetailAsync(UpdateUserDetailQuery query)
        {
            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var json = JsonConvert.SerializeObject(query);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await client.PostAsync(_baseUrl.AbsoluteUri + "/api/user/detail", data);

            if (!resp.IsSuccessStatusCode)
                throw new Exception(await resp.Content.ReadAsStringAsync());
        }

        public async Task DeleteUserAsync()
        {
            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var resp = await client.DeleteAsync(_baseUrl.AbsoluteUri + "/api/user");

            if (!resp.IsSuccessStatusCode)
                throw new Exception(await resp.Content.ReadAsStringAsync());
        }
    }
}
