using CompanyPortal.Application.Models.Employee;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Paging;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<PagedResult<EmployeeViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct);
    Task<EmployeeViewModel?> GetDetailAsync(int id, CancellationToken ct);
    Task<string> ChangeStatusAsync(int id, CancellationToken ct);
    Task<EmployeeCreateViewModel> GetForCreate(CancellationToken ct);
    Task CreateAsync(EmployeeCreateViewModel model, CancellationToken ct);
    Task<EmployeeUpdateViewModel> GetForUpdateAsync(int id, CancellationToken ct);
    Task UpdateAsync(int id, EmployeeUpdateViewModel model, CancellationToken ct);
    Task<EmployeePositionUpdateViewModel> GetForPositionUpdateAsync(int id, CancellationToken ct);
    Task UpdatePositionAsync(int id, EmployeePositionUpdateViewModel model, CancellationToken ct);
}
