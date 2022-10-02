using Newtonsoft.Json;

namespace ZillPillMobileApp.Domain.DTO.MedicalPoduct
{
    public class MedicalPoductDetailDto
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageData")]
        public byte[]? ImageData { get; set; }

        [JsonProperty("characteristics")]
        public string? Characteristics { get; set; }

        [JsonProperty("releases")]
        public List<string> Releases { get; set; }

        [JsonProperty("chemicals")]
        public List<string> Chemicals { get; set; }

        [JsonProperty("certificate")]
        public MedicalProductCertificateDto Certificate { get; set; }
    }

    public class MedicalProductCertificateDto
    {
        [JsonProperty("license")]
        public string? License { get; set; }

        [JsonProperty("registerDate")]
        public DateTime? RegisterDate { get; set; }

        [JsonProperty("approved")]
        public string? Approved { get; set; }
    }
}
