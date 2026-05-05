using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IDepartmentRepository : IBaseRepository<Department>
{
    IQueryable<Department> GetAllQueryableWithBuilding();
    Task<Department?> GetByIdAsync(int id, CancellationToken ct);
}
