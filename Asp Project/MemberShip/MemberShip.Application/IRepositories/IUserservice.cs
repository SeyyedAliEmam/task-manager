using MemberShip.Domain;

namespace MemberShip.Application.IRepositories
{
    public interface IUserservice
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int userid);
        User GetUserForDelete(int userid);
        bool IsExistUserByEmail(string email);
        User CheckEmailForLogin(string email);
        int GetUserCount();
        void InsertUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void DeleteUser(int userid);
        IEnumerable<User> GetAllUsers();
    }
}
