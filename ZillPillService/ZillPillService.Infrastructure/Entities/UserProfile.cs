using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZillPillService.Infrastructure.Entities
{
    [Index("Email", IsUnique = true, Name = "Email_Index")]
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? Email { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
