using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Models.ApplicationUser;
using CompanyPortal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.AppUser;

public sealed class ApplicationUserService(UserManager<ApplicationUser> userManager) : IApplicationUserService
{
    public async Task<ApplicationUserViewModel> GetInfoAsync(string applicationUserId, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(applicationUserId);
        if (user == null)
        {
            throw new EntityNotFoundException("Uživatel nebyl nalezen.");
        }

        var userVm = new ApplicationUserViewModel
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            DomainUserName = user.DomainUserName ?? string.Empty,
            IsEmployeeProfileCompleted = user.IsEmployeeProfileCompleted,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };

        return userVm;
    }

    public async Task<List<SelectListItem>> GetUsersForSelectListAsync(CancellationToken ct)
    {
        return await userManager.Users
            .Where(x => !x.IsEmployeeProfileCompleted && x.IsActive)
            .Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.UserName ?? x.Email ?? x.DomainUserName
            })
            .ToListAsync(ct);
    }

    public async Task MarkEmployeeProfileCompletedAsync(string applicationUserId, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(applicationUserId);
        if (user == null)
        {
            throw new EntityNotFoundException("Uživatel nebyl nalezen.");
        }

        user.IsEmployeeProfileCompleted = true;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception("Nepodařilo se aktualizovat uživatele.");
        }
    }
}
