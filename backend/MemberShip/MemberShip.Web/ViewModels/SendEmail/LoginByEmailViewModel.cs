using System.ComponentModel.DataAnnotations;

namespace MemberShip.Web.ViewModels.SendEmail
{
    public class LoginByEmailViewModel
    {
        [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
        [MaxLength(50, ErrorMessage = "تعداد کرکتر ها بیش از حد مجاز است")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل را رعایت کنید")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }
}
