using MemberShip.Application.IRepositories;
using MemberShip.Application.IUOF;
using MemberShip.Domain;
using MemberShip.Infrastructure.Repositories;

namespace MemberShip.Infrastructure.UOF
{
    public class Unitofwork : IUnitofwork
    {
        private readonly AppContext context;
        public Unitofwork(AppContext context)
        {
            this.context = context;
        }

        private IUserservice userservice;
        public IUserservice Userservice
        {
            get
            {
                if (userservice == null)
                {
                    userservice = new Userservice(new Genericrepository<User>(context));
                }
                return userservice;
            }
        }
        private ITodoservice todoservice;
        public ITodoservice Todoservice
        {
            get
            {
                if (todoservice == null)
                {
                    todoservice = new Todoservice(new Genericrepository<Todos>(context));
                }
                return todoservice;
            }
        }

        public void Dispose()
        {
            context?.Dispose();
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
