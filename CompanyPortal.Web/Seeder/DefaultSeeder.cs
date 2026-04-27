using CompanyPortal.Infrastructure.AppUser;
using Microsoft.AspNetCore.Identity;

namespace CompanyPortal.Web.Seeder;

public sealed class DefaultSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    public async Task SeedAdmiUserAsync()
    {

    }

    private async Task SeedDefaultRolesAsync()
    {

    }
}
