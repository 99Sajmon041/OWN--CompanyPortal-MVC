using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IEmployeeDeviceRepository : IBaseRepository<EmployeeDevice>
{
    IQueryable<EmployeeDevice> GetAllQueryableWithDependencies();
    Task<List<EmployeeDevice>> GetBorrowedByEmployeeIdAsync(int employeeId, CancellationToken ct);
}