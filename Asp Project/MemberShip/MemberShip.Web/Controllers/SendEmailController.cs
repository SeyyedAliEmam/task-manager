using MemberShip.Application.IEmailsender;
using MemberShip.Application.IUOF;
using MemberShip.Web.ViewModels.SendEmail;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace MemberShip.Web.Controllers
{
    public class SendEmailController : Controller
    {
        private readonly IEmailsender emailsender;
        private readonly IUnitofwork unitofwork;
        public SendEmailController(IEmailsender emailsender, IUnitofwork unitofwork)
        {
            this.emailsender = emailsender;
            this.unitofwork = unitofwork;
        }

        #region اکشن ورود به وسیله کد یکبار مصرف
        public ActionResult LoginByEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginByEmail(LoginByEmailViewModel loginbyemailvm)
        {
            if (ModelState.IsValid)
            {

                var user = unitofwork.Userservice.CheckEmailForLogin(loginbyemailvm.Email.ToLower());
                if (user == null)
                {
                    ModelState.AddModelError("", "کاربری با این ایمیل یافت نشد");
                    return View(loginbyemailvm);
                }

                try
                {
                    var OTPcode = GenerateOTPCode();
                    var subject = "کد ورود یک بار مصرف";
                    var message = $"کد شما برابر است با : {OTPcode}";

                    emailsender.SendEmail(loginbyemailvm.Email.ToLower(), subject, message);
                    TempData["OTPCode"] = OTPcode;
                    TempData["Email"] = loginbyemailvm.Email.ToLower();

                    return RedirectToAction("VerifyOTP");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "خطا در ارسال ایمیل");
                    return View(loginbyemailvm);
                }
            }

            return View(loginbyemailvm);
        }
        private string GenerateOTPCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        #endregion

        #region اکشن تایید کد ارسال شده
        public ActionResult VerifyOTP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyOTP(VerifyOTPViewModel verifyotpvm)
        {
            if (ModelState.IsValid)
            {
                var sroredotp = TempData["OTPCode"]?.ToString();
                var email = TempData["Email"]?.ToString();

                TempData.Remove("Email");
                TempData.Remove("OTPCode");

                if (verifyotpvm.OTPInput == sroredotp)
                {

                    var user = unitofwork.Userservice.CheckEmailForLogin(email);

                    if (!user.IsActive)
                    {
                        ModelState.AddModelError("", "حساب شما غیرفعال است");
                        return View(verifyotpvm);
                    }

                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("IsAdmin",user.IsAdmin.ToString())
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = true
                        };
                        HttpContext.SignInAsync(principal, properties);

                        return RedirectToAction("/");
                    }
                }
            }
            ModelState.AddModelError("", "کد ورودی نادرست است"); 
            return View(verifyotpvm);
        }
        #endregion

        #region ایمیل تبریک تولد
        public void SendBirthdayEmail()
        {
            var today = DateTime.Now;

            var users = unitofwork.Userservice.GetUsers();
            foreach (var user in users)
            {
                if (user.DateOfBirth.Month == today.Month && user.DateOfBirth.Day == today.Day)
                {
                    var subject = "تبریک تولد";
                    var message = $" کاربر با ایمیل {user.Email} تولدت مبارک";
                    emailsender.SendEmail(user.Email, subject, message);
                }
            }
        }
        #endregion

    }
}
