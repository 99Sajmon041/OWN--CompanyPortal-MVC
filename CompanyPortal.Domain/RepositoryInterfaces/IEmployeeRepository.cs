using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id, CancellationToken ct);
}
