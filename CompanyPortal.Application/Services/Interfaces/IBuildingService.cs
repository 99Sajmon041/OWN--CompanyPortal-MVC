using CompanyPortal.Application.Models.Building;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Paging;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IBuildingService
{
    Task<PagedResult<BuildingListItemViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct);
    Task CreateAsync(BuildingUpsertViewModel model, CancellationToken ct);
    Task UpdateAsync(int id, BuildingUpsertViewModel model, CancellationToken ct);
    Task<string> ChangeStatusAsync(int id, CancellationToken ct);
    Task<BuildingDetailViewModel> GetDetailAsync(int id, CancellationToken ct);
    Task<BuildingUpsertViewModel> GetForUpdateAsync(int id, CancellationToken ct);
}