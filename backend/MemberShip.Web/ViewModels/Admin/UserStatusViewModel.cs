namespace MemberShip.Web.ViewModels.Admin
{
    public class UserStatusViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
