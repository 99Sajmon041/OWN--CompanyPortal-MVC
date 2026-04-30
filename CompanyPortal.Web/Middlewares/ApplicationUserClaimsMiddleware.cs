using System.Security.Claims;
using CompanyPortal.Infrastructure.AppUser;
using CompanyPortal.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CompanyPortal.Web.Middlewares;

public sealed class ApplicationUserClaimsMiddleware(
    UserManager<ApplicationUser> userManager,
    CompanyPortalDbContext context,
    IOptions<List<string>> roleOptions) : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        string domainName = httpContext.User.Identity?.Name ?? "";

        if (string.IsNullOrWhiteSpace(domainName))
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        ApplicationUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.DomainUserName == domainName);

        if (user is null)
        {
            user = await CreateApplicationUserAsync(domainName);

            if (user is null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return;
            }
        }

        IList<string> roles = await userManager.GetRolesAsync(user);

        ClaimsIdentity identity = new ClaimsIdentity();

        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
        identity.AddClaim(new Claim("ApplicationUserId", user.Id));
        identity.AddClaim(new Claim("DomainUserName", user.DomainUserName));

        foreach (string role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        httpContext.User.AddIdentity(identity);

        await next(httpContext);
    }

    private async Task<ApplicationUser?> CreateApplicationUserAsync(string domainName)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        string userName = GetUserNameFromDomainName(domainName);

        ApplicationUser user = new ApplicationUser
        {
            UserName = userName,
            Email = $"{userName}@companyportal.local",
            DomainUserName = domainName,
            IsActive = true,
            IsEmployeeProfileCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        IdentityResult userResult = await userManager.CreateAsync(user);

        if (!userResult.Succeeded)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Failed to create application user. Error: {string.Join(", ", userResult.Errors.Select(x => x.Description))}");
            return null;
        }

        string defaultRole = roleOptions.Value.First(x => x == "Employee");

        IdentityResult roleResult = await userManager.AddToRoleAsync(user, defaultRole);

        if (!roleResult.Succeeded)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Failed to add default role to application user. Error: {string.Join(", ", roleResult.Errors.Select(x => x.Description))}");
            return null;
        }

        await transaction.CommitAsync();

        return user;
    }

    private static string GetUserNameFromDomainName(string domainName)
    {
        int slashIndex = domainName.LastIndexOf('\\');

        if (slashIndex >= 0)
        {
            return domainName.Substring(slashIndex + 1);
        }

        return domainName;
    }
}