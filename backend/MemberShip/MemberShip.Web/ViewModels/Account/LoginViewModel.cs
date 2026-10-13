using System.ComponentModel.DataAnnotations;

namespace MemberShip.Web.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
        [MaxLength(50 , ErrorMessage ="تعداد کرکتر ها بیش از حد مجاز است")]
        [EmailAddress (ErrorMessage ="فرمت ایمیل را رعایت کنید")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا کلمه عبور را وارد کنید")]
        [MaxLength(40,ErrorMessage ="تعداد کرکتر ها بیش از حد مجاز است")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
