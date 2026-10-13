using System.ComponentModel.DataAnnotations;

namespace MemberShip.Domain
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
