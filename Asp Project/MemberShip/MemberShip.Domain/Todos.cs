namespace MemberShip.Domain
{
    public class Todos : BaseEntity
    {
        public string Title { get; set; }
        public bool Completed { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
