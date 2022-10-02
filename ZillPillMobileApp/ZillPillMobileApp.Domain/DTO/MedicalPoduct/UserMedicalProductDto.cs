using Newtonsoft.Json;

namespace ZillPillMobileApp.Domain.DTO.MedicalPoduct
{
    public class UserMedicalProductDto : MedicalProductDto
    {
        [JsonProperty("relationId")]
        public int RelationId { get; set; }
    }
}
