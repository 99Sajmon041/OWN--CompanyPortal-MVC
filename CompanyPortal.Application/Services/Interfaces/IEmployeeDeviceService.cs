using CompanyPortal.Application.Models.EmployeeDevice;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Paging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IEmployeeDeviceService
{
    Task<PagedResult<EmployeeDeviceViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct);
    Task<List<EmployeeDeviceViewModel>> GetBorrowedByEmployeeIdAsync(int employeeId, CancellationToken ct);
    Task AssignDeviceAsync(int employeeId, int? deviceId, string? note, CancellationToken ct);
    Task MarkAsReturnedAsync(int employeeDeviceId, CancellationToken ct);
}