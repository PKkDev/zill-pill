using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ZillPillService.Application.Services
{
    public class ErrorNotificationService
    {
        private readonly ILogger<ErrorNotificationService> _logger;
        private IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ErrorNotificationService(
            ILogger<ErrorNotificationService> logger, IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendNotifToTelegram(string message)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var chanel = _configuration["TelegramConfig:ChatId"];
                var url = $"{_configuration["TelegramConfig:BaseUrl"]}?chanel={chanel}&message={message}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"{response.StatusCode}");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(SendNotifToTelegram)} - {e.Message}");
            }
        }
    }
}
