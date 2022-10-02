using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZillPillService.Infrastructure.Entities
{
    public class MedicinalProductChemical
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int MedicinalProductId { get; set; }
        public MedicinalProduct MedicinalProduct { get; set; }
    }
}
