using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class DeviceRepository(CompanyPortalDbContext context) : BaseRepository<Device>(context), IDeviceRepository
{
    private readonly CompanyPortalDbContext _context = context;

    public async Task<Device?> GetDetailByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Devices
            .AsNoTracking()
            .Include(x => x.EmployeeDevices)
            .ThenInclude(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<bool> HasAnyActiveBorrowingAsync(int id, CancellationToken ct)
    {
        return await _context.EmployeeDevices
            .AsNoTracking()
            .AnyAsync(x => x.DeviceId == id && x.ReturnedAt == null, ct);
    }
}