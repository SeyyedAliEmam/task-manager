using MemberShip.Application.IUOF;
using MemberShip.Domain;
using MemberShip.Web.ViewModels.Admin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MemberShip.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUnitofwork unitofwork;
        public AdminController(IUnitofwork unitofwork)
        {
            this.unitofwork = unitofwork;
        }

        #region اکشن مدیریت کاربران توسط ادمین
        public IActionResult UserManagement()
        {
            return View(unitofwork.Userservice.GetUsers());
        }
        #endregion

        #region اکشن تغییر وضعیت کاربران توسط ادمین 
        public IActionResult UserStatus(int id)
        {
            var user = unitofwork.Userservice.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            var userstatusvm = new UserStatusViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive,
                IsAdmin = user.IsAdmin,
            };

            return View(userstatusvm);
        }
        [HttpPost]
        public IActionResult UserStatus(int id, UserStatusViewModel userstatusvm)
        {
            if (id != userstatusvm.Id)
            {
                return NotFound();
            }
            var user = new User
            {
                Id = userstatusvm.Id,
                Email = userstatusvm.Email,
                Password = userstatusvm.Password,
                DateOfBirth = userstatusvm.DateOfBirth,
                IsActive = userstatusvm.IsActive,
                IsAdmin = userstatusvm.IsAdmin,
            };
            unitofwork.Userservice.UpdateUser(user);
            unitofwork.Save();

            var currentuser = HttpContext.User;
            if (currentuser.Identity.IsAuthenticated && currentuser.FindFirst(ClaimTypes.NameIdentifier).Value == user.Id.ToString())
            {
                if (!user.IsActive)
                {
                    HttpContext.SignOutAsync();
                }
                if(!user.IsAdmin)
                {
                    HttpContext.SignOutAsync();
                }
            }

            return RedirectToAction("UserManagement");
        }
        #endregion

        #region اکشن ایجاد کاربر توسط ادمین
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateViewModel createvm)
        {
            if (!ModelState.IsValid)
            {
                return View(createvm);
            }
            if (unitofwork.Userservice.IsExistUserByEmail(createvm.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "این ایمیل قبلا ثبت شده است");
                return View(createvm);
            }
            var hasher = new PasswordHasher();
            var hashedpassword = hasher.HashPassword(createvm.Password);
            var user = new User
            {
                Email = createvm.Email.ToLower(),
                Password = hashedpassword,
                DateOfBirth = createvm.DateOfBirth,
                IsActive = createvm.IsActive,
                IsAdmin = createvm.IsAdmin,
            };
            unitofwork.Userservice.InsertUser(user);
            unitofwork.Save();

            return RedirectToAction("UserManagement");
        }
        #endregion

        #region اکشن حذف کاربران توسط ادمین
        public IActionResult Delete(int id)
        {
            var user = unitofwork.Userservice.GetUserForDelete(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = unitofwork.Userservice.GetUserById(id);
            unitofwork.Userservice.DeleteUser(user);
            unitofwork.Save();
            return RedirectToAction("UserManagement");
        }
        #endregion
    }
}
