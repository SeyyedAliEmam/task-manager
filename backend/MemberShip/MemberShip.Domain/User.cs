using System.ComponentModel.DataAnnotations;

namespace MemberShip.Domain
{
    public class User : BaseEntity
    {

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(300)]
        public string Password { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }
        public ICollection<Todos> Todos { get; set; } = new List<Todos>();

    }
}
