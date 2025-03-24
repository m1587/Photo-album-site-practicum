using Bl.InterfaceServices;
using Dl.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return user != null ? Ok(new { user.Id, user.Name, user.Email }) : NotFound(new { Message = "User not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsersAsync();
                return Ok(users.Select(user => new { user.Id, user.Name, user.Email }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserPostModel user)
        {
            try
            {
                User userToAdd = new User { Name = user.Name,Email =user.Email,Password=user.Password };
                await _userService.AddUserAsync(userToAdd);
                return CreatedAtAction(nameof(GetUserById), new { id = userToAdd.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] Dl.Entities.LoginRequest loginRequest)
        {
            try
            {
                var user = await _userService.LoginUserAsync(loginRequest.Email, loginRequest.Password);

                if (user == null)
                    return Unauthorized(new { Message = "Invalid email or password" });
                var token = _userService.GenerateJwtToken(user.Name, new[] { user.Role.ToString() });
                return Ok(new { 
                    Message = "Login successful",
                    Token = token, 
                    User = new { user.Id, user.Name, user.Email }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserPostModel user)
        {
            try
            {
                User userToAdd = new User { Name = user.Name, Email = user.Email, Password = user.Password };
                var updatedUser = await _userService.UpdateUserAsync(id, userToAdd);

                if (updatedUser == null)
                    return NotFound(new { Message = "User not found" });

                return Ok(new { Message = "User updated successfully", User = updatedUser });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> RemoveUser(int id)
        {
            try
            {
                var deletedUser = await _userService.RemoveUserAsync(id);

                if (deletedUser == null)
                    return NotFound(new { Message = "User not found" });

                return Ok(new { Message = "User deleted successfully", User = deletedUser });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }


    }
}

