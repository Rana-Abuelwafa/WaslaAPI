using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wasla_Auth_App.Models;


namespace Wasla_Auth_App
{
    public class AuthenticationDBContext : IdentityDbContext<ApplicationUser>    {
        public AuthenticationDBContext(DbContextOptions<AuthenticationDBContext> options)
            : base(options)
        {
        }
    }
}
