using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Infrastructure.Database;
using CompanyPortal.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.AppUser;

public sealed class RoleAssignmentService(UserManager<ApplicationUser> userManager, CompanyPortalDbContext context) : IRoleAssignmentService
{
    public async Task<IEnumerable<string>> GetAllRolesAsync(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        return await context.Roles
            .Select(x => x.Name)
            .Where(x => x != null)
            .Select(x => x!)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new EntityNotFoundException("Uživatel nebyl nalezen.");
        }

        var roles = await userManager.GetRolesAsync(user);

        return roles;
    }

    public async Task AssignRolesByPositionLevelAsync(string userId, PositionLevel positionLevel, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
