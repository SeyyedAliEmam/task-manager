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
    [ApiController]
    [Route("api/[controller]")]
    public class AccountApiController : ControllerBase
    {
        private readonly IUnitofwork unitofwork;

        public AccountApiController(IUnitofwork unitofwork)
        {
            this.unitofwork = unitofwork;
        }

        #region اکشن ثبت نام کاربر برای React
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterViewModel registervm)
        {
            if (!ModelState.IsValid)
            {
                // برگرداندن خطاهای اعتبارسنجی فرم به React
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "اطلاعات وارد شده نامعتبر است", errors });
            }

            if (unitofwork.Userservice.IsExistUserByEmail(registervm.Email.ToLower()))
            {
                return BadRequest(new { message = "این ایمیل قبلا ثبت شده است" });
            }

            var hasher = new PasswordHasher();
            var hashedpassword = hasher.HashPassword(registervm.Password);

            User user;
            if (unitofwork.Userservice.GetUserCount() == 0)
            {
                user = new User()
                {
                    Email = registervm.Email.ToLower(),
                    Password = hashedpassword,
                    DateOfBirth = registervm.DateOfBirth,
                    IsAdmin = true,
                    IsActive = true,
                };
            }
            else
            {
                user = new User()
                {
                    Email = registervm.Email.ToLower(),
                    Password = hashedpassword,
                    DateOfBirth = registervm.DateOfBirth,
                    IsAdmin = false,
                    IsActive = true,
                };
            }

            unitofwork.Userservice.InsertUser(user);
            return Ok(new { message = "ثبت نام با موفقیت انجام شد" });
        }
        #endregion

        #region اکشن ورود کاربر برای React
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginvm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "لطفا ایمیل و کلمه عبور را به درستی وارد کنید" });
            }

            var user = unitofwork.Userservice.CheckEmailForLogin(loginvm.Email.ToLower());

            if (user == null)
            {
                return BadRequest(new { message = "اطلاعات وارد شده صحیح نیست" });
            }

            if (!user.IsActive)
            {
                return BadRequest(new { message = "حساب شما غیرفعال است" });
            }

            var hasher = new PasswordHasher();
            if (hasher.VerifyHashedPassword(user.Password, loginvm.Password) == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("IsAdmin", user.IsAdmin.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = loginvm.RememberMe
                };

                // ثبت کوکی در مرورگر
                await HttpContext.SignInAsync(principal, properties);

                // برگرداندن اطلاعات کاربر به React جهت ذخیره در Context
                return Ok(new
                {
                    id = user.Id,
                    email = user.Email,
                    isAdmin = user.IsAdmin
                });
            }

            return BadRequest(new { message = "اطلاعات وارد شده صحیح نیست" });
        }
        #endregion

        #region اکشن بررسی وضعیت فعلی کاربر (برای بارگذاری مجدد React)
        // این متد بررسی می‌کند که آیا کاربر از قبل کوکی معتبر دارد یا خیر
        [HttpGet("current-user")]
        public IActionResult GetCurrentUser()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                var isAdminClaim = User.FindFirst("IsAdmin")?.Value;
                bool isAdmin = isAdminClaim != null && bool.Parse(isAdminClaim);

                // پیدا کردن شناسه کاربر از روی Claims
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return Ok(new
                {
                    isAuthenticated = true,
                    user = new { id = userId, email, isAdmin }
                });
            }

            return Ok(new { isAuthenticated = false });
        }
        #endregion

        #region اکشن خروج از حساب کاربری برای React
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "با موفقیت خارج شدید" });
        }
        #endregion
    }
}

