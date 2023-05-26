using GraduationProjectAPI.DataTransferObject;

namespace GraduationProjectAPI.Services.User
{
    public interface IUserService
    {
        public Task<GraduationProjectAPI.Models.User?> Get(int id);
        public Task<GraduationProjectAPI.Models.User?> ForgotPassword(ForgotPasswordModel forgotPassword);
        public Task<bool> ChangePassword(int userId, ChangePasswordModel changePassword);
        public Task<bool> Register(RegisterModel addUserModel);
        public Task<bool> DeleteAUser(string email);



    }
}
