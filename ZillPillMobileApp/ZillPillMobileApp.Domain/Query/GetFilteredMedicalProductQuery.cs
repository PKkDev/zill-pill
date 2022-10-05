namespace ZillPillMobileApp.Domain.Query
{
    public class GetFilteredMedicalProductQuery
    {
        public int? CountryId { get; set; }
        public string? ProductName { get; set; }
        public bool? WithCertificate { get; set; }
        public string? ChemicalName { get; set; }

        public GetFilteredMedicalProductQuery()
        {
            CountryId = null;
            ProductName = null;
            WithCertificate = null;
            ChemicalName = null;
        }

        public GetFilteredMedicalProductQuery(int? countryId, string? productName, bool? withCertificate, string? chemicalName)
        {
            CountryId = countryId;
            ProductName = productName;
            WithCertificate = withCertificate;
            ChemicalName = chemicalName;
        }
    }
}
