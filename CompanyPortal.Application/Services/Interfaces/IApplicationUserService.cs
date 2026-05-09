using CompanyPortal.Application.Models.ApplicationUser;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IApplicationUserService
{
    Task<List<SelectListItem>> GetUsersForSelectListAsync(CancellationToken ct);
    Task MarkEmployeeProfileCompletedAsync(string applicationUserId, CancellationToken ct);
    Task<ApplicationUserViewModel> GetInfoAsync(string applicationUserId, CancellationToken ct);
}
