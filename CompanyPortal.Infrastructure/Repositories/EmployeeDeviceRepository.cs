using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class EmployeeDeviceRepository(CompanyPortalDbContext context) : BaseRepository<EmployeeDevice>(context), IEmployeeDeviceRepository
{
    private readonly CompanyPortalDbContext _context = context;

    public IQueryable<EmployeeDevice> GetAllQueryableWithDependencies()
    {
        return _context.EmployeeDevices
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Device);
    }

    public async Task<bool> IsDeviceCurrentlyBorrowedAsync(int deviceId, CancellationToken ct)
    {
        return await _context.EmployeeDevices
            .AsNoTracking()
            .AnyAsync(x => x.DeviceId == deviceId && x.ReturnedAt == null, ct);
    }
}