using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZillPillService.Infrastructure.Entities
{
    public class MedicinalProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public byte[]? Data { get; set; }

        public int MedicinalProductId { get; set; }
        public MedicinalProduct MedicinalProduct { get; set; }
    }
}
