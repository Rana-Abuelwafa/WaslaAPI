using Microsoft.EntityFrameworkCore;

namespace Wasla_Auth_App
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AuthenticationDBContext context = scope.ServiceProvider.GetRequiredService<AuthenticationDBContext>();

            context.Database.Migrate();
        }
    }
}
