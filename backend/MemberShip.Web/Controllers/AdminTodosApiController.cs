using MemberShip.Application.IUOF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemberShip.Web.Controllers
{
    [Authorize(Policy = "AdminOnly")] 
    [ApiController]
    [Route("api/[controller]")]
    public class AdminTodosApiController : ControllerBase
    {
        private readonly IUnitofwork unitofwork;

        public AdminTodosApiController(IUnitofwork unitofwork)
        {
            this.unitofwork = unitofwork;
        }

        [HttpGet]
        public IActionResult GetAllTodos()
        {

            var todos = unitofwork.Todoservice.GetAllTodos(); 

            var todosDto = todos.Select(t => new
            {
                id = t.Id,
                title = t.Title,
                completed = t.Completed,
                userId = t.UserId 
            });

            return Ok(todosDto);
        }
    }
}

