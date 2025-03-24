using Bl.InterfaceServices;
using Bl.validation;
using Dl;
using Dl.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Bl.Services
{
    public class UserService : IUserService
    {
        private readonly IDataContext _dataContext;
        private readonly IConfiguration _configuration;
        public UserService(IDataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }
        public string GenerateJwtToken(string username, string[] roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, username)
            };

            // הוספת תפקידים כ-Claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            UserValidation.ValidateUserId(id);
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            return user ?? throw new KeyNotFoundException("User not found");
        }
        public async Task<List<User>> GetUsersAsync() => await _dataContext.Users.ToListAsync();
        public async Task AddUserAsync(User user)
        {
            UserValidation.ValidateUserId(user.Id);
            UserValidation.ValidateUserName(user.Name);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<User> LoginUserAsync(string email, string password)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }
            return user;
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            UserValidation.ValidateUserId(id);
            UserValidation.ValidateUserId(user.Id);

            var existingUser = await GetUserByIdAsync(id);
            if (existingUser == null)
                throw new KeyNotFoundException("User not found");

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            if (!string.IsNullOrEmpty(user.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }
            await _dataContext.SaveChangesAsync();
            return existingUser;
        }
        

        public async Task<User> RemoveUserAsync(int id)
        {
            UserValidation.ValidateUserId(id);
            var userToDelete = await GetUserByIdAsync(id);
            if (userToDelete == null)
                throw new KeyNotFoundException("User not found");
            userToDelete.IsDeleted = true;
            await UpdateUserAsync(userToDelete.Id, userToDelete);
 
            return userToDelete;
        }
    }
}
