using GraduationProjectAPI.DataTransferObject;
using GraduationProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GraduationProjectAPI.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly DatabaseContext _dbContext;
        public AuthService(DatabaseContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public string GenerateToken(string id,string email ,string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, id), //Id của user
                new Claim(ClaimTypes.Role, role), // claim role
                new Claim(ClaimTypes.Email, email), // claim role

                 }),
                Expires = DateTime.Now.AddDays(double.Parse(_config["Jwt:ExpireDays"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<Models.User?> AuthenticateUser(LoginModel login)
        {
            try
            {
                Models.User? user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);
                return user;
            }
            catch
            {
                throw;
            }
        }

    }
}
