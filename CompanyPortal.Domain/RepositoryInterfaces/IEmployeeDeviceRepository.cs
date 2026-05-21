using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IEmployeeDeviceRepository : IBaseRepository<EmployeeDevice>
{
    IQueryable<EmployeeDevice> GetAllQueryableWithDependencies();
    Task<bool> IsDeviceCurrentlyBorrowedAsync(int deviceId, CancellationToken ct);
}