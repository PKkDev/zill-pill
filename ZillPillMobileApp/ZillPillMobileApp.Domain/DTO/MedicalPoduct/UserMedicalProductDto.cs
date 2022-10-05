using Newtonsoft.Json;

namespace ZillPillMobileApp.Domain.DTO.MedicalPoduct
{
    public class UserMedicalProductDto : MedicalProductDto
    {
        [JsonProperty("relationId")]
        public int RelationId { get; set; }

        [JsonProperty("totalToAccept")]
        public int TotalToAccept { get; set; }

        [JsonProperty("totalAccepted")]
        public int TotalAccepted { get; set; }

        [JsonProperty("progress")]
        public double Progress { get; set; }
    }
}
