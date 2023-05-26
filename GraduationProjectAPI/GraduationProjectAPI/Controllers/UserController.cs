using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GraduationProjectAPI.DataTransferObject;
using GraduationProjectAPI.Services.User;
using GraduationProjectAPI.Services.Context;
using GraduationProjectAPI.Services.Email;
using System.Text.RegularExpressions;

namespace GraduationProjectAPI.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    
    public class UserController
    {
        private readonly IUserService _userService;
        private readonly IHttpContextMethod _httpContextMethod;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmailService _emailService;



        public UserController(IUserService userService, IWebHostEnvironment hostEnvironment, IHttpContextMethod httpContextMethod, IEmailService sendEmail)
        {
            _userService = userService;
            _hostingEnvironment = hostEnvironment;
            _httpContextMethod = httpContextMethod;
            _emailService = sendEmail;

        }

        [HttpGet("Info")]
        [Authorize]
        public async Task<IActionResult> Info()
        {
            try
            {
                int idContext = _httpContextMethod.GetIDContext();

                Models.User? user = idContext != 0 ? await _userService.Get(idContext) : null;

                var objectUser = user != null ? new { user.FullName, user.Email, user.PhoneNumber, user.Address, user.Avatar } : null;
                return new OkObjectResult(new APIResponse<object>(objectUser, "success", true));

            }
            catch (Exception e)
            {
                return new NotFoundObjectResult(new APIResponse<object>(null, e.Message, false));
            }

        }



        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel forgotPassword)
        {
            if (string.IsNullOrEmpty(forgotPassword.Email))
            {
                return new BadRequestObjectResult(new APIResponse<string>(null, "Username không được để trống.", false));
            }

            try
            {
                GraduationProjectAPI.Models.User? user = await _userService.ForgotPassword(forgotPassword);
                if (user != null)
                {
                    bool flog = await _emailService.SendEmailFromGmail(user.Email, "XWay", $"Đặt lại mật khẩu thành công, mật khẩu mới của bạn là: {user.Password}.");
                    if (flog == true)
                    {
                        return new OkObjectResult(new APIResponse<bool>(true, $"Tạo tài khoản thành công, mật khẩu mới đã được gửi về email {user.Email}.", true));
                    }
                    else
                    {
                        return new BadRequestObjectResult(new APIResponse<bool>(false, "Đặt lại mật khẩu thành công, mật khẩu chưa được gửi về email.", false));
                    }
                }
                else
                {
                    return new NotFoundObjectResult(new APIResponse<bool>(false, "Đặt lại mật khẩu thất bại.", false));
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new APIResponse<bool>(false, e.Message, false));
            }
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel changePassword)
        {

            int idContext = _httpContextMethod.GetIDContext();
            if (idContext == 0)
            {
                return new UnauthorizedResult();
            }

            if (string.IsNullOrEmpty(changePassword.Password))
            {
                return new BadRequestObjectResult(new APIResponse<string>(null, "Mật khẩu mới không được để trống.", false));
            }

            try
            {
                bool isChange = await _userService.ChangePassword(idContext,changePassword);

                if (isChange == true)
                {
                    return new OkObjectResult(new APIResponse<bool>(true, "Thay đổi mật khẩu thành công.", true));
                }
                else
                {
                    return new BadRequestObjectResult(new APIResponse<bool>(false, "Thay đổi mật khẩu thất bại.", false));
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new APIResponse<bool>(false, e.Message, false));
            }
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
        {

            if (string.IsNullOrEmpty(register.Password) || string.IsNullOrEmpty(register.Email))
            {
                return new BadRequestObjectResult(new APIResponse<string>(null, "Email hoặc mật khẩu không được để trống.", false));
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (Regex.IsMatch(register.Email, pattern))
            {
                try
                {
                    bool isRegister = await _userService.Register(register);
                    if (isRegister == true)
                    {
                        bool flog = await _emailService.SendEmailFromGmail(register.Email, "XWay", $"Đăng ký tài khoản thành công.");
                        if (flog == true)
                        {
                            return new OkObjectResult(new APIResponse<bool>(true, $"Tạo tài khoản thành công.", true));
                        }
                        else
                        {
                            await _userService.DeleteAUser(register.Email);
                            return new NotFoundObjectResult(new APIResponse<bool>(false, "Lỗi hệ thống, vui lòng đăng ký lại sau.", false));
                        }
                    }
                    else
                    {
                        return new NotFoundObjectResult(new APIResponse<bool>(false, "Tạo tài khoản thất bại.", false));
                    }
                }
                catch
                {
                    return new BadRequestObjectResult(new APIResponse<bool>(false, "Tạo tài khoản thất bại.", false));
                }
            }
            else
            {
                return new BadRequestObjectResult(new APIResponse<bool>(false, "Email không hợp lệ", false));
            }

        }



    }
}
