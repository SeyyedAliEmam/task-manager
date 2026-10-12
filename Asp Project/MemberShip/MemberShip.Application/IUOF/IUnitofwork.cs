using MemberShip.Application.IRepositories;

namespace MemberShip.Application.IUOF
{
    public interface IUnitofwork : IDisposable
    {
        IUserservice Userservice { get; }
        ITodoservice Todoservice { get; }
        void Save();
    }
}
