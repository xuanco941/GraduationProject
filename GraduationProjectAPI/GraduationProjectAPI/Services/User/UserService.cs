using Microsoft.EntityFrameworkCore;
using GraduationProjectAPI.DataTransferObject;
using GraduationProjectAPI.Models;

namespace GraduationProjectAPI.Services.User
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _dbContext;

        public UserService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<GraduationProjectAPI.Models.User?> Get(int id)
        {
            try
            {
                GraduationProjectAPI.Models.User? u = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserID == id);
                return u;
            }
            catch
            {
                throw;
            }
        }
        

        public async Task<GraduationProjectAPI.Models.User?> ForgotPassword(ForgotPasswordModel forgotPassword)
        {
            Random rand = new Random();
            string password = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                password = password + rand.Next(0, 10);
            }

            try
            {
                GraduationProjectAPI.Models.User? user = _dbContext.Users.FirstOrDefault(a => a.Email == forgotPassword.Email);
                if (user != null)
                {
                    user.Password = password;
                }
                int num = await _dbContext.SaveChangesAsync();
                if (num > 0)
                {
                    return user;
                }
                else
                {
                    return null;
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ChangePassword(int userId, ChangePasswordModel changePassword)
        {
            try
            {
                GraduationProjectAPI.Models.User? user = await _dbContext.Users.FirstOrDefaultAsync(a => a.UserID == userId);
                if (user != null)
                {
                    user.Password = changePassword.Password;
                }
                return await _dbContext.SaveChangesAsync() > 0;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {
            GraduationProjectAPI.Models.User user = new GraduationProjectAPI.Models.User()
            {

                Email = registerModel.Email,
                Password = registerModel.Password
            };

            try
            {
                await _dbContext.AddAsync(user);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteAUser(string email)
        {
            try
            {
                var user = await _dbContext.Users
                .FirstOrDefaultAsync(c => c.Email == email);
                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    return await _dbContext.SaveChangesAsync() > 0;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }




    }

}
