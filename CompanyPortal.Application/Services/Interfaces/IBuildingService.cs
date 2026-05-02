using CompanyPortal.Application.Models.Building;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IBuildingService
{
    // List - dynamicsPager
    Task CreateAsync(BuildingUpsertViewModel model, CancellationToken ct);
    Task UpdateAsync(BuildingUpsertViewModel model, CancellationToken ct);
    Task ChangeStatusAsync(int id, CancellationToken ct);
    Task<BuildingDetailViewModel> GetDetailAsync(int id, CancellationToken ct);
}