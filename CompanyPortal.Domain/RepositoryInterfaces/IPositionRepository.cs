using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IPositionRepository : IBaseRepository<Position>
{
    IQueryable<Position> GetAllQueryableWithDepartment();
    Task<Position?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsAnyWithSameParametersAsync(string positionName, int id, CancellationToken ct);
    Task<bool> ExistsAnyWithSameParametersAsync(string positionName, CancellationToken ct);
    Task<bool> ExistsManagingPositionInDepartmentAsync(int departmentId, CancellationToken ct);
    Task<List<Position>> GetAllAsyncWithPositions(CancellationToken ct);
}