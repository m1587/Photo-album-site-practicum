using Dl.Entities;
namespace Bl.InterfaceServices
{
    public interface IUserService
    {
        public string GenerateJwtToken(string username, string[] roles);
        public Task<User> GetUserByIdAsync(int id);
        public Task<List<User>> GetUsersAsync();
        public Task AddUserAsync(User user);

        public Task<User> LoginUserAsync(string email, string password);
        public Task<User> UpdateUserAsync(int id, User user);
        public Task<User> RemoveUserAsync(int id);


    }

}
