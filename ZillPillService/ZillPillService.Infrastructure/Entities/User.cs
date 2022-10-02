using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZillPillService.Infrastructure.Entities
{
    [Index("Phone", IsUnique = true, Name = "Phone_Index")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? Code { get; set; }

        public UserProfile Profile { get; set; }

        public List<MedicinalProduct> MedicinalProducts { get; set; }
        public List<UserMedicinalProduct> UserMedicinalProduct { get; set; }

        public User()
        {
            MedicinalProducts = new();
            UserMedicinalProduct = new();
        }
    }
}
