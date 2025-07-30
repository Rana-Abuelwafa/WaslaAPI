namespace Wasla_Auth_App.Models
{
    public class UsersWithRoles : ApplicationUser
    {
        public string? Roles { get; set; }
        
    }

    public class UsersWithRolesGrp
    {
        public string? Roles { get; set; }
        public int count { get; set; }
        public List<ApplicationUser> users { get; set; }

    }



}
