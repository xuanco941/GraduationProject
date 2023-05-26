using System.Security.Claims;

namespace GraduationProjectAPI.Services.Context
{
    public class HttpContextMethod : IHttpContextMethod
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextMethod(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int GetIDContext()
        {

            var httpContext = _httpContextAccessor.HttpContext;
            string? Id = null;
            if (httpContext != null && httpContext.User != null)
            {
                Id = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }

            if (string.IsNullOrEmpty(Id) == false)
            {
                try
                {
                    return int.Parse(Id);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public string? GetEmailFromContext()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.User != null)
            {
                var emailClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (emailClaim != null)
                {
                    return emailClaim.Value;
                }
            }

            return null;
        }
    }
}
