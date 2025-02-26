using Microsoft.AspNetCore.Identity;

namespace Wasla_Auth_App.Models
{
    public class User : ApplicationUser
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? msg { get; set; }
        public bool isSuccessed { get; set; }


    }
}
