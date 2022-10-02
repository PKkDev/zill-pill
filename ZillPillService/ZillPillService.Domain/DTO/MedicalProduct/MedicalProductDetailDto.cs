namespace ZillPillService.Domain.DTO.MedicalProduct
{
    public class MedicalProductDetailDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public byte[]? ImageData { get; set; }

        public string? Characteristics { get; set; }

        public List<string> Releases { get; set; }

        public List<string> Chemicals { get; set; }

        public MedicalProductCertificateDto Certificate { get; set; }
    }

    public class MedicalProductCertificateDto
    {
        public string? License { get; set; }

        public DateTime? RegisterDate { get; set; }

        public string? Approved { get; set; }

        public MedicalProductCertificateDto(string? license, DateTime? registerDate, string? approved)
        {
            License = license;
            RegisterDate = registerDate;
            Approved = approved;
        }
    }
}
