using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IEmployeeRepository : IBaseRepository<Employee>
{
    Task<Employee?> GetByIdAsync(int id, CancellationToken ct);
    IQueryable<Employee> GetAllWithPositionAsync();
    Task<(bool, int)> ExistsWithSameNameAsync(string firstName, string lastName, CancellationToken ct);
}
