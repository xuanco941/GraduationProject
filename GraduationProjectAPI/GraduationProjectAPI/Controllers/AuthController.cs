using GraduationProjectAPI.DataTransferObject;
using GraduationProjectAPI.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProjectAPI.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class AuthController
    {

        private readonly IAuthService _authService;



        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] LoginModel loginModel)
        {
            //kiểm tra tài khoản mật khẩu gửi xuống có rỗng hoặc null không
            if (string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {
                return new BadRequestObjectResult(new APIResponse<string>(null, "Email hoặc password không được để trống.", false));
            }

            try
            {
                var u = await _authService.AuthenticateUser(loginModel);
                if (u != null)
                {
                    var tokenString = _authService.GenerateToken(u.UserID.ToString(),u.Email, "User");

                    return new OkObjectResult(new APIResponse<string>(tokenString, "Xác thực thành công", true));
                }
                else
                {
                    return new NotFoundResult();
                }


            }
            catch (Exception e)
            {
                return new OkObjectResult(new APIResponse<string>("error", e.Message, true));
            }
        }


    }
}
