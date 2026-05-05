using CompanyPortal.Application.Models.Department;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Paging;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IDepartmentService
{
    Task<PagedResult<DepartmentListItemViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct);
    Task<DepartmentDetailViewModel> GetDetailAsync(int id, CancellationToken ct);
}