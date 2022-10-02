using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZillPillService.Infrastructure.Entities
{
    public class MedicinalProductCertificate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? License { get; set; }

        public DateTime? RegisterDate { get; set; }

        public string? Approved { get; set; }

        public int MedicinalProductId { get; set; }
        public MedicinalProduct MedicinalProduct { get; set; }
    }
}
