using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IBuildingRepository : IBaseRepository<Building>
{
    Task<Building?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsAnyWithSameParametersAsync(string buildingName, string code, CancellationToken ct);
    Task<bool> ExistsAnyWithSameParametersAsync(string buildingName, string code, int id, CancellationToken ct);
}
