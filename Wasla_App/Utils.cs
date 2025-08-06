using System.Security.Claims;

namespace Wasla_App
{
    public class Utils
    {
        public static LoginUserData getTokenData(IHttpContextAccessor _httpContextAccessor)
        {
            LoginUserData userData = new LoginUserData();
            string? clientId = string.Empty;
            string? email = string.Empty;
            if (_httpContextAccessor.HttpContext is not null)
            {
                clientId = _httpContextAccessor.HttpContext.User.FindFirstValue("ClientId");
                email = _httpContextAccessor.HttpContext.User.FindFirstValue("Email");
               
            }
            userData.client_id = clientId;
            userData.client_email = email;
            return userData;
        }
    }
}
