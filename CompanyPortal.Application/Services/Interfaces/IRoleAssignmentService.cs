using CompanyPortal.Shared.Enums;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IRoleAssignmentService
{
    Task<IEnumerable<string>> GetUserRolesAsync(string userId, CancellationToken ct);
    Task<IEnumerable<string>> GetAllRolesAsync(CancellationToken ct);
    Task AssignRolesByPositionLevelAsync(string userId, PositionLevel positionLevel, CancellationToken ct);
}
