using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IDeviceRepository : IBaseRepository<Device>
{
    Task<Device?> GetDetailByIdAsync(int id, CancellationToken ct);
    Task<bool> HasAnyActiveBorrowingAsync(int id, CancellationToken ct);
    Task<bool> ExistsAnyWithSameSnAsync(string serialNumber, CancellationToken ct);
    Task<bool> ExistsAnyWithSameSnAsync(string serialNumber, int id, CancellationToken ct);
}
