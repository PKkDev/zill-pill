namespace ZillPillService.Domain.Query
{
    public class GetFilteredMedicalProductQuery
    {
        public int? CountryId { get; set; }
        public string? ProductName { get; set; }
        public bool? WithCertificate { get; set; }
        public string? ChemicalName { get; set; }
    }
}
