using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IEmployeeRepository : IBaseRepository<Employee>
{
    Task<Employee?> GetByIdAsync(int id, CancellationToken ct);
    Task<Employee?> GetByApplicationUserIdAsync(string applicationUserId, CancellationToken ct);
    IQueryable<Employee> GetAllWithPositionAsync();
    Task<bool> ExistsActiveEmployeeOnPositionAsync(int positionId, int excludedEmployeeId, CancellationToken ct);
    Task<bool> ExistsActiveEmployeeOnPositionAsync(int positionId, CancellationToken ct);
    Task<Employee?> GetByApplicationUserIdWithDependenciesAsync(string applicationUserId, CancellationToken ct);
    Task<List<Employee>> GetAllByDepartmentAsync(int departmentId, int currentEmployeeId, CancellationToken ct);
    Task<(string, int)> GetGroupLeaderAsync(int departmentId, CancellationToken ct);
}
