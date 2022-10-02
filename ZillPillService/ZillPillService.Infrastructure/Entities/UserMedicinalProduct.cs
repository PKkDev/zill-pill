using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ZillPillService.Domain.Model;

namespace ZillPillService.Infrastructure.Entities
{
    public class UserMedicinalProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int MedicinalProductId { get; set; }
        public MedicinalProduct MedicinalProduct { get; set; }

        public ShedullerType? ShedullerType { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public List<MedicationSheduller> Shedullers { get; set; }

        public UserMedicinalProduct()
        {
            Shedullers = new();
        }
    }
}
