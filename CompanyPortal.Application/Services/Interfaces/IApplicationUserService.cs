using CompanyPortal.Application.Models.ApplicationUser;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Paging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IApplicationUserService
{
    Task<List<SelectListItem>> GetUsersForSelectListAsync(CancellationToken ct);
    Task MarkEmployeeProfileCompletedAsync(string applicationUserId, CancellationToken ct);
    Task<ApplicationUserViewModel> GetInfoAsync(string applicationUserId, CancellationToken ct);
    Task<PagedResult<ApplicationUserViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct);
    Task<string> ChangeStatusAsync(string userId, CancellationToken ct);
}
