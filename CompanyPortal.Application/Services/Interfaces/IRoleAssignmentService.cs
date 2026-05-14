using CompanyPortal.Shared.Authorization;
using CompanyPortal.Shared.Enums;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IRoleAssignmentService
{
    Task<IEnumerable<string>> GetUserRolesAsync(string userId, CancellationToken ct);
    Task<IEnumerable<string>> GetAllRolesAsync(CancellationToken ct);
    Task SyncRolesByPositionLevelAsync(string userId, PositionLevel positionLevel, CancellationToken ct);
    Task AssignRoleAsync(string userId, string role, CancellationToken ct);
}
