namespace ZillPillMobileApp.Domain.DTO.Countries
{
    public class CountryDetailDto
    {
        public int CountryId { get; set; }

        public string Name { get; set; }

        public byte[]? ImageData { get; set; }
    }
}
