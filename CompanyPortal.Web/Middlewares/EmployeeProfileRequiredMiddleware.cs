using CompanyPortal.Infrastructure.AppUser;
using CompanyPortal.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyPortal.Web.Middlewares;

public sealed class EmployeeProfileRequiredMiddleware(CompanyPortalDbContext context, UserManager<ApplicationUser> userManager) : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        string? applicationUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var applicationUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == applicationUserId);

        if (string.IsNullOrWhiteSpace(applicationUserId) || applicationUser is null || applicationUser.AccessValidUntil <= DateTime.UtcNow)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        bool employeeExists = await context.Employees.AnyAsync(x => x.ApplicationUserId == applicationUserId);

        if (!employeeExists && !IsAllowedPath(httpContext))
        {
            httpContext.Response.Redirect("/Access/EmployeeProfileRequired");
            return;
        }

        bool isEmployeeActive = await context.Employees.AnyAsync(x => x.ApplicationUserId == applicationUserId && x.IsActive);

        if (!isEmployeeActive && !IsAllowedPath(httpContext))
        {
            httpContext.Response.Redirect("/Access/EmployeeProfileNotActive");
            return;
        }

        await next(httpContext);
    }

    private static bool IsAllowedPath(HttpContext httpContext)
    {
        PathString path = httpContext.Request.Path;

        return path.StartsWithSegments("/Access/EmployeeProfileRequired")
            || path.StartsWithSegments("/css")
            || path.StartsWithSegments("/js")
            || path.StartsWithSegments("/lib")
            || path.StartsWithSegments("/Access/EmployeeProfileNotActive")
            || path.StartsWithSegments("/favicon.ico");
    }
}
