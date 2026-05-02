using CompanyPortal.Application.Models.Building;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IBuildingService
{
    // List - dynamicsPager
    Task CreateAsync(BuildingUpsertViewModel model, CancellationToken ct);
    Task UpdateAsync(BuildingUpsertViewModel model, CancellationToken ct);
    Task<string> ChangeStatusAsync(int id, CancellationToken ct);
    Task<BuildingDetailViewModel> GetDetailAsync(int id, CancellationToken ct);
    Task<BuildingUpsertViewModel> GetForUpdateAsync(int id, CancellationToken ct);
}