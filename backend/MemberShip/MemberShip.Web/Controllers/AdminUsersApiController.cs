using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MemberShip.Application.IUOF;
using MemberShip.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemberShip.Web.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminUsersApiController : ControllerBase
    {
        private readonly IUnitofwork unitofwork;

        public AdminUsersApiController(IUnitofwork unitofwork)
        {
            this.unitofwork = unitofwork;
        }

        #region دریافت لیست کاربران

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = unitofwork.Userservice.GetAllUsers();

            var usersDto = users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                IsAdmin = u.IsAdmin
            });

            return Ok(usersDto);
        }

        #endregion

        #region افزودن کاربر توسط ادمین

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto model)
        {
            if (model == null)
            {
                return BadRequest(new
                {
                    message = "اطلاعات کاربر ارسال نشده است."
                });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    message = "اطلاعات وارد شده معتبر نیستند.",
                    errors
                });
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest(new
                {
                    message = "ایمیل الزامی است."
                });
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new
                {
                    message = "رمز عبور الزامی است."
                });
            }

            var normalizedEmail = model.Email.Trim().ToLowerInvariant();

            if (unitofwork.Userservice.IsExistUserByEmail(normalizedEmail))
            {
                return Conflict(new
                {
                    message = "کاربری با این ایمیل از قبل وجود دارد."
                });
            }


            var hasher = new PasswordHasher();
            var hashedPassword = hasher.HashPassword(model.Password);

            var newUser = new User
            {
                Email = normalizedEmail,
                Password = hashedPassword,
                IsAdmin = model.IsAdmin,
                IsActive = true
            };

            unitofwork.Userservice.InsertUser(newUser);

            var userDto = new UserDto
            {
                Id = newUser.Id,
                Email = newUser.Email,
                IsAdmin = newUser.IsAdmin
            };

            return CreatedAtAction(
                nameof(GetAllUsers),
                new { id = newUser.Id },
                userDto
            );
        }

        #endregion

        #region ویرایش کاربر توسط ادمین

        [HttpPut("{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto model)
        {
            if (model == null)
            {
                return BadRequest(new
                {
                    message = "اطلاعات کاربر ارسال نشده است."
                });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    message = "اطلاعات وارد شده معتبر نیستند.",
                    errors
                });
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest(new
                {
                    message = "ایمیل الزامی است."
                });
            }

            var userToUpdate = unitofwork.Userservice.GetUserById(id);

            if (userToUpdate == null)
            {
                return NotFound(new
                {
                    message = "کاربر مورد نظر پیدا نشد."
                });
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.Equals(currentUserId, id.ToString()) && !model.IsAdmin)
            {
                return StatusCode(403, new
                {
                    message = "شما نمی‌توانید نقش ادمین حساب خود را لغو کنید."
                });
            }

            var normalizedEmail = model.Email.Trim().ToLowerInvariant();

            var emailIsUsedByAnotherUser = unitofwork.Userservice
                .GetAllUsers()
                .Any(u =>
                    !string.IsNullOrWhiteSpace(u.Email) &&
                    u.Email.Trim().ToLowerInvariant() == normalizedEmail &&
                    u.Id != id
                );

            if (emailIsUsedByAnotherUser)
            {
                return Conflict(new
                {
                    message = "کاربری با این ایمیل از قبل وجود دارد."
                });
            }

            userToUpdate.Email = normalizedEmail;
            userToUpdate.IsAdmin = model.IsAdmin;


            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var hasher = new PasswordHasher();

                userToUpdate.Password = hasher.HashPassword(model.Password);
            }

            unitofwork.Userservice.UpdateUser(userToUpdate);

            return Ok(new UserDto
            {
                Id = userToUpdate.Id,
                Email = userToUpdate.Email,
                IsAdmin = userToUpdate.IsAdmin
            });
        }

        #endregion

        #region حذف کاربر توسط ادمین

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.Equals(currentUserId, id.ToString()))
                {
                    return StatusCode(403, new
                    {
                        message = "شما نمی‌توانید حساب کاربری خود را حذف کنید."
                    });
                }

                var userToDelete = unitofwork.Userservice.GetUserById(id);

                if (userToDelete == null)
                {
                    return NotFound(new
                    {
                        message = "کاربر مورد نظر پیدا نشد."
                    });
                }

                // اول تسک‌های وابسته به کاربر حذف می‌شوند
                var userTodos = unitofwork.Todoservice
                    .GetTodosByUserId(id)
                    .ToList();

                foreach (var todo in userTodos)
                {
                    unitofwork.Todoservice.DeleteTodo(todo);
                }

                // بعد خود کاربر حذف می‌شود
                unitofwork.Userservice.DeleteUser(id);

                return Ok(new
                {
                    success = true,
                    message = "کاربر و تسک‌های مربوط به او با موفقیت حذف شدند."
                });
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;

                return StatusCode(500, new
                {
                    message = "خطا در حذف کاربر از دیتابیس: " + errorMessage
                });
            }
        }


        #endregion
    }

    public class UserDto
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }
    }

    public class CreateUserDto
    {
        [Required(ErrorMessage = "ایمیل الزامی است.")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل معتبر نیست.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور الزامی است.")]
        public string Password { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }
    }

    public class UpdateUserDto
    {
        [Required(ErrorMessage = "ایمیل الزامی است.")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل معتبر نیست.")]
        public string Email { get; set; } = string.Empty;

        public string? Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}



