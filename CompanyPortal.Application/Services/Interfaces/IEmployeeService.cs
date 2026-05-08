using CompanyPortal.Application.Models.Employee;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Paging;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<PagedResult<EmployeeViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct);
    Task<EmployeeViewModel?> GetByIdAsync(int id, CancellationToken ct);
    Task<string> ChangeStatusAsync(int id, CancellationToken ct);
    Task<EmployeeUpsertViewModel> GetForCreate(CancellationToken ct);
    Task CreateAsync(EmployeeUpsertViewModel model, CancellationToken ct);
}
