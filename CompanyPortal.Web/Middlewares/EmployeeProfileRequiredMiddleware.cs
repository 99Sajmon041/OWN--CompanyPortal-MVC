using CompanyPortal.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyPortal.Web.Middlewares;

public sealed class EmployeeProfileRequiredMiddleware(CompanyPortalDbContext context) : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        string? applicationUserId = httpContext.User.FindFirstValue("ApplicationUserId"); //ClaimTypes.NameIdentifier

        if (string.IsNullOrWhiteSpace(applicationUserId))
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        bool employeeExists = await context.Employees.AnyAsync(x => x.ApplicationUserId == applicationUserId);
    
        if (!employeeExists && !IsAllowedPath(httpContext))
        {
            httpContext.Response.Redirect("/Access/EmployeeProfileRequired");
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
            || path.StartsWithSegments("/favicon.ico");
    }
}
