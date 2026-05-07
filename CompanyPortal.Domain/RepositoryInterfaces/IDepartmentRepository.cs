using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IDepartmentRepository : IBaseRepository<Department>
{
    Task<bool> ExistsAnyWithSameParametersAsync(string departmentName, int id, CancellationToken ct);
    Task<bool> ExistsAnyWithSameParametersAsync(string departmentName, CancellationToken ct);
    IQueryable<Department> GetAllQueryableWithBuilding();
    Task<Department?> GetByIdAsync(int id, CancellationToken ct);
}
