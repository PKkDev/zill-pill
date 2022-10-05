namespace ZillPillService.Domain.DTO.MedicalProduct
{
    public class UserMedicalProductDto : MedicalProductDto
    {
        public int RelationId { get; set; }

        public int TotalToAccept { get; set; }
        public int TotalAccepted { get; set; }
        public double Progress { get; set; }

        public UserMedicalProductDto(int productId, string name, byte[]? imageData, int relationId)
            : base(productId, name, imageData)
        {
            RelationId = relationId;
        }
    }
}
