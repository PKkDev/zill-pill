using Newtonsoft.Json;

namespace ZillPillMobileApp.Domain.DTO.MedicalPoduct
{
    public class MedicalProductDto
    {
        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageData")]
        public byte[]? ImageData { get; set; }
    }
}
