using Newtonsoft.Json;

namespace ZillPillMobileApp.Domain.DTO.Shedullers
{
    public class ShedullerItemDetailDto
    {
        [JsonProperty("shedullerItemId")]
        public int ShedullerItemId { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("time")]
        public TimeSpan Time { get; set; }

        [JsonProperty("quantity")]
        public double Quantity { get; set; }

        [JsonProperty("isSended")]
        public bool IsSended { get; set; }

        [JsonProperty("isAccepted")]
        public bool IsAccepted { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("imageData")]
        public byte[] ImageData { get; set; }
    }
}
