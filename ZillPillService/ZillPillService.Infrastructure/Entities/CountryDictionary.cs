using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZillPillService.Infrastructure.Entities
{
    public class CountryDictionary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public byte[]? ImageData { get; set; }

        public List<MedicinalProduct> MedicinalProducts { get; set; }

        public CountryDictionary()
        {
            MedicinalProducts = new();
        }
    }
}
