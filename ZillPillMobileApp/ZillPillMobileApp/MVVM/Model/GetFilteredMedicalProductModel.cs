using ZillPillMobileApp.Core;

namespace ZillPillMobileApp.MVVM.Model
{
    public class GetFilteredMedicalProductModel : ObservableObject
    {
        public int? CountryId { get; set; }

        public string? ProductName { get; set; }

        public bool? WithCertificate { get; set; }

        public string? ChemicalName { get; set; }

        public GetFilteredMedicalProductModel()
        {
            CountryId = null;
            ProductName = null;
            WithCertificate = true;
            ChemicalName = null;
        }
    }
}
