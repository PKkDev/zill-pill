namespace ZillPillService.Domain.DTO.Countries
{
    public class CountryDetailDto
    {
        public int CountryId { get; set; }

        public string Name { get; set; }

        public byte[]? ImageData { get; set; }

        public CountryDetailDto(int countryId, string name, byte[]? imageData)
        {
            CountryId = countryId;
            Name = name;
            ImageData = imageData;
        }
    }
}
