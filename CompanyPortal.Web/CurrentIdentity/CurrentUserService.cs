using System.Security.Claims;
using CompanyPortal.Application.CurrentIdentity;

namespace CompanyPortal.Web.CurrentIdentity;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public string UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    public string DomainUserName => User?.FindFirstValue("DomainUserName") ?? string.Empty;

    public IReadOnlyCollection<string> Roles =>
        User?.Claims
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .ToList() ?? [];

    public bool IsInRole(string role)
    {
        return User?.IsInRole(role) ?? false;
    }
}