using System.Security.Claims;
using MemberShip.Application.IUOF;
using MemberShip.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MemberShip.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodosApiController : ControllerBase
    {
        private readonly IUnitofwork unitofwork;

        public TodosApiController(IUnitofwork unitofwork)
        {
            this.unitofwork = unitofwork;
        }

        [HttpGet]
        public IActionResult GetMyTodos()
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return Unauthorized(new
                {
                    message = "کاربر وارد نشده است."
                });
            }

            var todos = unitofwork.Todoservice
                .GetTodosByUserId(userId.Value)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    completed = t.Completed,
                    userId = t.UserId
                });

            return Ok(todos);
        }

        [HttpPost]
        public IActionResult CreateTodo([FromBody] CreateTodoDto model)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return Unauthorized(new
                {
                    message = "کاربر وارد نشده است."
                });
            }

            if (model == null || string.IsNullOrWhiteSpace(model.Title))
            {
                return BadRequest(new
                {
                    message = "عنوان تسک الزامی است."
                });
            }

            var todo = new Todos
            {
                Title = model.Title.Trim(),
                Completed = false,
                UserId = userId.Value
            };

            unitofwork.Todoservice.InsertTodo(todo);

            return Ok(new
            {
                id = todo.Id,
                title = todo.Title,
                completed = todo.Completed,
                userId = todo.UserId
            });
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateTodoStatus(int id, [FromBody] UpdateTodoStatusDto model)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return Unauthorized(new
                {
                    message = "کاربر وارد نشده است."
                });
            }

            var todo = unitofwork.Todoservice.GetTodoById(id);

            if (todo == null)
            {
                return NotFound(new
                {
                    message = "تسک پیدا نشد."
                });
            }

            if (todo.UserId != userId.Value)
            {
                return Forbid();
            }

            todo.Completed = model.Completed;
            unitofwork.Todoservice.UpdateTodo(todo);

            return Ok(new
            {
                id = todo.Id,
                title = todo.Title,
                completed = todo.Completed,
                userId = todo.UserId
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return Unauthorized(new
                {
                    message = "کاربر وارد نشده است."
                });
            }

            var todo = unitofwork.Todoservice.GetTodoById(id);

            if (todo == null)
            {
                return NotFound(new
                {
                    message = "تسک پیدا نشد."
                });
            }

            if (todo.UserId != userId.Value)
            {
                return Forbid();
            }

            unitofwork.Todoservice.DeleteTodo(todo);

            return Ok(new
            {
                success = true,
                message = "تسک با موفقیت حذف شد."
            });
        }

        [HttpPost("import-jsonplaceholder")]
        public async Task<IActionResult> ImportJsonPlaceholderTodos()
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId == null)
            {
                return Unauthorized(new
                {
                    message = "کاربر وارد نشده است."
                });
            }

            var alreadyImported = unitofwork.Todoservice
                .GetAllTodos()
                .Any(t => t.Title.StartsWith("[JSONPlaceholder]"));

            if (alreadyImported)
            {
                return BadRequest(new
                {
                    message = "داده‌های JSONPlaceholder قبلاً وارد دیتابیس شده‌اند."
                });
            }

            using var httpClient = new HttpClient();

            var externalTodos = await httpClient.GetFromJsonAsync<List<JsonPlaceholderTodoDto>>(
                "https://jsonplaceholder.typicode.com/todos"
            );

            if (externalTodos == null || !externalTodos.Any())
            {
                return BadRequest(new
                {
                    message = "دریافت داده‌ها از JSONPlaceholder ناموفق بود."
                });
            }

            foreach (var item in externalTodos)
            {
                var todo = new Todos
                {
                    Title = $"[JSONPlaceholder] {item.Title}",
                    Completed = item.Completed,
                    UserId = item.UserId
                };

                unitofwork.Todoservice.InsertTodo(todo);
            }

            return Ok(new
            {
                success = true,
                message = $"{externalTodos.Count} تسک با موفقیت بین کاربران 1 تا 10 پخش شد."
            });
        }



        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return null;
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return null;
            }

            return userId;
        }
    }

    public class CreateTodoDto
    {
        public string Title { get; set; }
    }

    public class JsonPlaceholderTodoDto
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }

    public class UpdateTodoStatusDto
    {
        public bool Completed { get; set; }
    }
}

