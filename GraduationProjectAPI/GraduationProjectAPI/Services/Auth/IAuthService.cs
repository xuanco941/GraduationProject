
using GraduationProjectAPI.DataTransferObject;

namespace GraduationProjectAPI.Services.Auth
{
    public interface IAuthService
    {
        public string GenerateToken(string id, string email, string role);
        public Task<Models.User?> AuthenticateUser(LoginModel login);

    }
}
