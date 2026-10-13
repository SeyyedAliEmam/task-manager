using MemberShip.Domain;

namespace MemberShip.Application.IRepositories
{
    public interface ITodoservice
    {
        IEnumerable<Todos> GetTodosByUserId(int userId);

        Todos GetTodoById(int todoId);

        IEnumerable<Todos> GetAllTodos();

        void InsertTodo(Todos todo);

        void UpdateTodo(Todos todo);

        void DeleteTodo(Todos todo);

        void DeleteTodo(int todoId);
    }
}

