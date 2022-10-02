using Newtonsoft.Json;
using System.Text;
using ZillPillMobileApp.Domain.DTO.User;
using ZillPillMobileApp.Domain.Query.User;

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

            //var query = new PhoneAuthorizeQuery(phone);
            //var api = RestService.For<IUserApiContract>(_baseUrl.AbsoluteUri);
            //await api.SendAccesTokenToSms(query);
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

            //var query = new CheckPhoneAuthorizeQuery(phone, code);
            //var api = RestService.For<IUserApiContract>(_baseUrl.AbsoluteUri);
            //var response = await api.CheckPhoneAccessToken(query);
            //return response;
        }

        public async Task<UserDetailDto> GetUserDetailAsync()
        {
            var client = new HttpClient(httpClientHandler);
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var resp = await client.GetAsync(_baseUrl.AbsoluteUri + "/api/user/detail");

            var response = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new Exception(response);

            return JsonConvert.DeserializeObject<UserDetailDto>(response);
        }
    }
}
