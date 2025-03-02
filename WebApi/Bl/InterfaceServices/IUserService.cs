using Dl.Entities;
namespace Bl.InterfaceServices
{
    public interface IUserService
    {
        public User GetUserById(int id);
        public List<User> GetUsers();
        public void AddUser(User user);
        public void UpdateUser(int id, User user);
        public void RemoveUser(int id);


    }

}
