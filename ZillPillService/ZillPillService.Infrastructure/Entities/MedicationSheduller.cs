using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZillPillService.Infrastructure.Entities
{
    public class MedicationSheduller
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime UnionUtcDate { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public double Quantity { get; set; }

        public bool IsSended { get; set; }

        public bool IsAccepted { get; set; }

        public int UserMedicinalProductId { get; set; }
        public UserMedicinalProduct UserMedicinalProduct { get; set; }
    }
}
