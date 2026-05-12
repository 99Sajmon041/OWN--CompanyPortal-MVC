using CompanyPortal.Application.CurrentIdentity;
using CompanyPortal.Shared.Enums;
using System.Security.Claims;

namespace CompanyPortal.Web.CurrentIdentity;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;
    public string UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    public string DomainUserName => User?.FindFirstValue("DomainUserName") ?? string.Empty;
    public int EmployeeId => int.TryParse(User?.FindFirstValue("EmployeeId"), out var id) ? id : 0;
    public int PositionId => int.TryParse(User?.FindFirstValue("PositionId"), out var id) ? id : 0;
    public int DepartmentId => int.TryParse(User?.FindFirstValue("DepartmentId"), out var id) ? id : 0;
    public PositionLevel? PositionLevel => Enum.TryParse(User?.FindFirstValue("PositionLevel"), out PositionLevel level) ? level : null;

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