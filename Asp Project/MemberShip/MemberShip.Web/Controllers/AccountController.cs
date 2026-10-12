using MemberShip.Application.IUOF;
using MemberShip.Domain;
using MemberShip.Web.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc; 
using System.Security.Claims;

namespace MemberShip.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitofwork unitofwork;
        public AccountController(IUnitofwork unitofwork)
        {
            this.unitofwork = unitofwork;
        }

        #region اکشن ثبت نام کاربر
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost] 
        public IActionResult Register(RegisterViewModel registervm)
        {

            if(!ModelState.IsValid)
            {
                return View(registervm);
            }
            if(unitofwork.Userservice.IsExistUserByEmail(registervm.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "این ایمیل قبلا ثبت شده است");
                return View(registervm);
            }
            var hasher = new PasswordHasher();
            var hashedpassword = hasher.HashPassword(registervm.Password);
            if (unitofwork.Userservice.GetUserCount() == 0)
            {
                User user = new User()
                {
                    Email = registervm.Email.ToLower(),
                    Password = hashedpassword,
                    DateOfBirth = registervm.DateOfBirth,
                    IsAdmin = true,
                    IsActive = true,
                };
                unitofwork.Userservice.InsertUser(user);
            }
            else
            {
                User user = new User()
                {
                    Email = registervm.Email.ToLower(),
                    Password = hashedpassword,
                    DateOfBirth = registervm.DateOfBirth,
                    IsAdmin = false,
                    IsActive = true,
                };
                unitofwork.Userservice.InsertUser(user);
            }

            return View("SuccessRegister", registervm);
        }
        #endregion

        #region اکشن ورود کاربر
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginvm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginvm);
            }

            var user = unitofwork.Userservice.CheckEmailForLogin(loginvm.Email.ToLower());

            if (user == null)
            {
                ModelState.AddModelError("", "اطلاعات وارد شده صحیح نیست");
                return View(loginvm);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "حساب شما غیرفعال است");
                return View(loginvm);
            }

            var hasher = new PasswordHasher();
            if (hasher.VerifyHashedPassword(user.Password, loginvm.Password) == PasswordVerificationResult.Success)
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
                    IsPersistent = loginvm.RememberMe
                };
                HttpContext.SignInAsync(principal, properties);

                return Redirect("/");
            }
            else
            {
                ModelState.AddModelError("", "اطلاعات وارد شده صحیح نیست");
                return View(loginvm);
            }
        }
        #endregion

        #region اکشن خروج از حساب کاربری
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //دستورات مربوط به لاگ اوت شدن یوزر 
            return Redirect("/Account/Login");
            //برگرد به اکشن لاگین
        }
        #endregion

    }
}
