using CompanyPortal.Application.Models.Department;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Paging;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IDepartmentService
{
    Task<PagedResult<DepartmentListItemViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct);
    Task CreateAsync(DepartmentUpsertViewModel model, CancellationToken ct);
    Task<DepartmentDetailViewModel> GetDetailAsync(int id, CancellationToken ct);
    Task<DepartmentUpsertViewModel> GetForCreateAsync(CancellationToken ct);
    Task<DepartmentUpsertViewModel> GetForUpdateAsync(int id, CancellationToken ct);
    Task UpdateAsync(int id, DepartmentUpsertViewModel model, CancellationToken ct);
    Task<string> ChangeStatusAsync(int id, CancellationToken ct);
}