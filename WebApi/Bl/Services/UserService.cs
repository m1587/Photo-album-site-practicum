using Bl.InterfaceServices;
using Dl;
using Dl.Entities;
using System.Xml.Linq;

namespace Bl.Services
{
    internal class UserService:IUserService
    {
        private readonly IDataContext _dataContext;
        public UserService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public User GetUserById(int id)
        {
            return _dataContext.Users.FirstOrDefault(x => x.Id == id);
        }
        public List<User> GetUsers()
        {
            return _dataContext.Users.ToList();
        }
        public void AddUser(User user)
        {
                _dataContext.Users.Add(user);
                _dataContext.SaveChanges();
        }
        public void UpdateUser(int id, User user)
        {
            var newUser = _dataContext.Users.Where(user => user.Id == id).FirstOrDefault();
            if (newUser != null)
            {
                newUser.Name = user.Name;
                newUser.Email = user.Email;
                newUser.Password = user.Password;
                newUser.UpdatedBy = user.UpdatedBy;
                newUser.UpdatedAt = user.UpdatedAt;
                _dataContext.SaveChanges();
            }
        }
        public void RemoveUser(int id)
        {
            var userToDelete = _dataContext.Users.FirstOrDefault(baby => baby.Id == id);
            if (userToDelete != null)
            {
                _dataContext.Users.Remove(userToDelete);
                _dataContext.SaveChanges();
            }
        }
    }
}
