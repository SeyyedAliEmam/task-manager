using System.ComponentModel.DataAnnotations;

namespace MemberShip.Web.ViewModels.SendEmail
{
    public class VerifyOTPViewModel
    {
        [Required(ErrorMessage ="لطفا کد یکبار مصرف را وارد کنید")]
        public string OTPInput {  get; set; }
    }
}
