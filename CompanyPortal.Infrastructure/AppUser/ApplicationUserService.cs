using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyPortal.Application.Exceptions;
using CompanyPortal.Application.Models.ApplicationUser;
using CompanyPortal.Application.Models.Department;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Shared.Enums;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Filtering;
using CompanyPortal.Shared.Grid.Paging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.AppUser;

public sealed class ApplicationUserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : IApplicationUserService
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
            CreatedAt = user.CreatedAt,
            AccessValidUntil = user.AccessValidUntil
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

    public async Task<PagedResult<ApplicationUserViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct)
    {
        gridRequest.Normalize();

        IQueryable<ApplicationUser> query = userManager.Users;

        query = query.ApplyFilters(gridRequest.Filters);

        int totalItems = await query.CountAsync(ct);

        var pager = new Pager(totalItems, gridRequest.Page, gridRequest.PageSize);

        query = query.ApplySorting(gridRequest.SortOrder);
        query = query.ApplyPaging(pager);

        var items = await query.Select(x => new ApplicationUserViewModel
        {
            Id = x.Id,
            UserName = x.UserName ?? string.Empty,
            Email = x.Email ?? string.Empty,
            DomainUserName = x.DomainUserName ?? string.Empty,
            IsEmployeeProfileCompleted = x.IsEmployeeProfileCompleted,
            IsActive = x.IsActive,
            CreatedAt = x.CreatedAt,
            AccessValidUntil = x.AccessValidUntil
        })
        .ToListAsync(ct);

        return new PagedResult<ApplicationUserViewModel>
        {
            Items = items,
            Pager = pager,
            GridRequest = gridRequest,
            FilterModelType = typeof(ApplicationUser)
        };
    }

    public async Task<string> ChangeStatusAsync(string userId, CancellationToken ct)
    {
        var appUser = await userManager.FindByIdAsync(userId);
        if (appUser is null)
        {
            throw new EntityNotFoundException("Uživatel nebyl nalezen.");
        }

        var employee = await unitOfWork.EmployeeRepository.GetByApplicationUserIdAsync(userId, ct);
        if (employee is not null && employee.IsActive)
        {
            throw new Exception("Zaměstnanec vedený pod tímto uživatelským účtem je stále aktivní, nejdříve jej deaktivujte jako zaměstnance.");
        }

        if (employee is not null && employee.Position.PositionLevel == PositionLevel.Administrator)
        {
            throw new Exception("Není možné upravit administrátora.");
        }

        string term;

        if (appUser.AccessValidUntil <= DateTime.UtcNow)
        {
            throw new Exception($"Uživatel byl již deaktivován k datu: {appUser.AccessValidUntil}.");
        }
        else if (appUser.AccessValidUntil is not null)
        {
            var daysToDeactivate = (appUser.AccessValidUntil.Value - DateTime.UtcNow).Days;
            throw new Exception($"Uživatel již bude deaktivován za: {daysToDeactivate} dní.");
        }
        else
        {
            var deactivatedDate = DateTime.UtcNow.AddMonths(2);
            appUser.AccessValidUntil = deactivatedDate;

            var result = await userManager.UpdateAsync(appUser);

            if (!result.Succeeded)
            {
                throw new Exception("Nepodařilo se deaktivovat uživatele.");
            }

            term = deactivatedDate.ToString("dd.MM.yyyy");
        }

        return term;
    }
}
