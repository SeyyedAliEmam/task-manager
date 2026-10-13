using System.ComponentModel.DataAnnotations;

namespace MemberShip.Web.ViewModels.Admin
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
        [MaxLength(50, ErrorMessage = "تعداد کرکتر ها بیش از حد مجاز است")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل را رعایت کنید")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا کلمه عبور را وارد کنید")]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا تاریخ تولد خود را وارد کنید")]
        [DataType(DataType.DateTime)]
        [Display(Name = "تاریخ تولد")]
        public DateTime DateOfBirth { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
