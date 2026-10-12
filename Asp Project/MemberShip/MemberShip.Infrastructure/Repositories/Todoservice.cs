using MemberShip.Application.IRepositories;
using MemberShip.Domain;

namespace MemberShip.Infrastructure.Repositories
{
    public class Todoservice : ITodoservice
    {
        private readonly IGenericrepository<Todos> todorepository;

        public Todoservice(IGenericrepository<Todos> todorepository)
        {
            this.todorepository = todorepository;
        }

        public IEnumerable<Todos> GetTodosByUserId(int userId)
        {
            return todorepository.Get(t => t.UserId == userId);
        }
        public IEnumerable<Todos> GetAllTodos()
        {
            return todorepository.Get(null);
        }

        public Todos GetTodoById(int todoId)
        {
            return todorepository.GetEntityById(todoId);
        }

        public void InsertTodo(Todos todo)
        {
            todorepository.Insert(todo);
        }

        public void UpdateTodo(Todos todo)
        {
            todorepository.Update(todo);
        }

        public void DeleteTodo(Todos todo)
        {
            todorepository.Delete(todo);
        }

        public void DeleteTodo(int todoId)
        {
            var todo = todorepository.GetEntityById(todoId);
            if (todo != null)
            {
                todorepository.Delete(todo);
            }
        }
    }
}

