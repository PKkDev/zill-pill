namespace ZillPillService.Domain.DTO.MedicalProduct
{
    public class MedicalProductDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public byte[]? ImageData { get; set; }

        public MedicalProductDto(int productId, string name, byte[]? imageData)
        {
            ProductId = productId;
            ImageData = imageData;
            Name = name;
        }
    }
}
