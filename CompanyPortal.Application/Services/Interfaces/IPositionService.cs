using CompanyPortal.Application.Models.Position;
using CompanyPortal.Shared.Grid.Common;
using CompanyPortal.Shared.Grid.Paging;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IPositionService
{
    Task<PagedResult<PositionListItemViewModel>> GetAllPagedAsync(GridRequest gridRequest, CancellationToken ct);
    Task CreateAsync(PositionUpsertViewModel model, CancellationToken ct);
    Task<PositionDetailViewModel> GetDetailAsync(int id, CancellationToken ct);
    Task<PositionUpsertViewModel> GetForCreateAsync(CancellationToken ct);
    Task<PositionUpsertViewModel> GetForUpdateAsync(int id, CancellationToken ct);
    Task UpdateAsync(int id, PositionUpsertViewModel model, CancellationToken ct);
    Task<string> ChangeStatusAsync(int id, CancellationToken ct);
}
