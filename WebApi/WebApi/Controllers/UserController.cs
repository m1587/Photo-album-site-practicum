using Bl.InterfaceServices;
using Dl.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet("{id}")]
        public User GetUserById(int id)
        {
            return userService.GetUserById(id);
        }
        [HttpGet]
        public List<User> GetUsers()
        {
            return userService.GetUsers();
        }
        [HttpPost]
        public void AddUser([FromBody] User user)
        {
            userService.AddUser(user);
        }
        [HttpPut("{id}")]
        public void UpdateUser(int id, [FromBody] User user)
        {
            userService.UpdateUser(id, user);
        }
        [HttpDelete("{id}")]
        public void RemoveUser(int id)
        {
            userService.RemoveUser(id);
        }
    }

}
