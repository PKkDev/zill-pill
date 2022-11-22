using Newtonsoft.Json;

namespace ZillPillMobileApp.Domain.DTO.Error
{
    public class HttpErrorMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        public HttpErrorMessage() { }
    }
}
