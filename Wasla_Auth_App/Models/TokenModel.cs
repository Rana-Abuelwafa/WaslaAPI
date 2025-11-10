namespace Wasla_Auth_App.Models
{
    public class TokenModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? email { get; set; }
    }
}
