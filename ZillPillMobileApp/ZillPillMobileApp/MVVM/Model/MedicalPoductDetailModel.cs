namespace ZillPillMobileApp.MVVM.Model
{
    public class MedicalPoductDetailModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ImageSource ImageSource { get; set; }

        public string? Characteristics { get; set; }

        public List<string> Releases { get; set; }

        public List<string> Chemicals { get; set; }

        public MedicalProductCertificateModel Certificate { get; set; }
    }

    public class MedicalProductCertificateModel
    {
        public string? License { get; set; }

        public DateTime? RegisterDate { get; set; }

        public string? Approved { get; set; }
    }
}
