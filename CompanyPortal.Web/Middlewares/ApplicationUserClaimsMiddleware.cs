using System.Security.Claims;
using CompanyPortal.Infrastructure.AppUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CompanyPortal.Web.Middlewares;

public sealed class ApplicationUserClaimsMiddleware(UserManager<ApplicationUser> userManager, IOptions<List<string>> roleOptions) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string domainName = context.User.Identity?.Name ?? "";

        if (string.IsNullOrWhiteSpace(domainName))
        {
            await next(context);
            return;
        }

        ApplicationUser? user = await userManager.Users
            .FirstOrDefaultAsync(x => x.DomainUserName == domainName);

        if (user is null)
        {
            int slashIndex = domainName.LastIndexOf('\\');

            string userName = slashIndex >= 0
                ? domainName.Substring(slashIndex + 1)
                : domainName;

            user = new ApplicationUser
            {
                UserName = userName,
                Email = $"{userName}@companyportal.local",
                DomainUserName = domainName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            IdentityResult userResult = await userManager.CreateAsync(user);

            if (!userResult.Succeeded)
            {
                await next(context);
                return;
            }

            string defaultRole = roleOptions.Value.First(x => x == "Employee");
            await userManager.AddToRoleAsync(user, defaultRole);
        }

        IList<string> roles = await userManager.GetRolesAsync(user);

        ClaimsIdentity identity = new ClaimsIdentity();

        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
        identity.AddClaim(new Claim("DomainUserName", user.DomainUserName));

        foreach (string role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        context.User.AddIdentity(identity);

        await next(context);
    }
}