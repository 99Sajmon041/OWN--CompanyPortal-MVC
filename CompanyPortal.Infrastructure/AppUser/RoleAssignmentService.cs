using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Infrastructure.Database;
using CompanyPortal.Shared.Authorization;
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
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new EntityNotFoundException("Uživatel nebyl nalezen.");
        }

        var roles = new List<string> { AppRoles.Employee };

        if (positionLevel == PositionLevel.GroupLeader)
        {
            roles.Add(AppRoles.GroupLeader);
        }

        var currentRoles = await GetUserRolesAsync(userId, ct);
        var rolesToAdd = roles.Except(currentRoles).ToList();

        if (rolesToAdd.Count == 0)
        {
            return;
        }

        var roleResult = await userManager.AddToRolesAsync(user, rolesToAdd);
        
        if (!roleResult.Succeeded)
        {
            throw new Exception($"Role se nepodařilo přiřadit uživateli. Error: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
        }
    }

    public async Task AssignRoleAsync(string userId, string role, CancellationToken ct)
    {
        bool roleExists = await context.Roles.AnyAsync(x => x.Name == role, ct);
        if (!roleExists)
        {
            throw new EntityNotFoundException("Role nebyla nalezena.");
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new EntityNotFoundException("Uživatel nebyl nalezen.");
        }

        var currentRoles = await GetUserRolesAsync(userId, ct);
        if (currentRoles.Contains(role))
        {
            return;
        }

        var roleResult = await userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
        {
            throw new Exception($"Roli se uživateli nepodařilo přiřadit. Error: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
        }
    }
}