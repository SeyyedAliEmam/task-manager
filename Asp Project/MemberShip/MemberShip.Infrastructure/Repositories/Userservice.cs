using MemberShip.Application.IRepositories;
using MemberShip.Domain;

namespace MemberShip.Infrastructure.Repositories
{
    public class Userservice : IUserservice
    {
        private readonly IGenericrepository<User> userrepository;

        public Userservice(IGenericrepository<User> userrepository)
        {
            this.userrepository = userrepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return userrepository.Get(null);
        }

        public IEnumerable<User> GetUsers()
        {
            return userrepository.Get(null);
        }

        public User GetUserById(int userid)
        {
            return userrepository.GetEntityById(userid);
        }

        public User GetUserForDelete(int userid)
        {
            var user = userrepository
                .Get(u => u.Id == userid)
                .FirstOrDefault();

            return user;
        }

        public bool IsExistUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var normalizedEmail = email.Trim().ToLowerInvariant();

            return userrepository.IsExist(u => u.Email == normalizedEmail);
        }

        public User CheckEmailForLogin(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var normalizedEmail = email.Trim().ToLowerInvariant();

            var user = userrepository
                .Get(u => u.Email == normalizedEmail)
                .SingleOrDefault();

            return user;
        }

        public int GetUserCount()
        {
            return userrepository
                .Get(u => true)
                .Count();
        }

        public void InsertUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                user.Email = user.Email.Trim().ToLowerInvariant();
            }

            userrepository.Insert(user);
        }

        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            userrepository.Delete(user);
        }

        public void DeleteUser(int userid)
        {
            var user = userrepository.GetEntityById(userid);

            if (user != null)
            {
                userrepository.Delete(user);
            }
        }

        public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                user.Email = user.Email.Trim().ToLowerInvariant();
            }

            userrepository.Update(user);
        }

    }
}



