using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZillPillService.Infrastructure.Entities
{
    public class MedicinalProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Manufacturer { get; set; }

        public string? Characteristics { get; set; }

        public List<MedicinalProductChemical> Chemicals { get; set; }

        public List<MedicinalProductRelease> Releases { get; set; }

        public MedicinalProductImage Image { get; set; }

        public MedicinalProductCertificate Certificate { get; set; }

        public List<User> Users { get; set; }
        public List<UserMedicinalProduct> UserMedicinalProduct { get; set; }

        public MedicinalProduct()
        {
            Chemicals = new();
            Releases = new();
            UserMedicinalProduct = new();
            Users = new();
        }
    }
}
