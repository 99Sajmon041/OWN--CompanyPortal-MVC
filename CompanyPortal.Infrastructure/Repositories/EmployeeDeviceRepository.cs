using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class EmployeeDeviceRepository(CompanyPortalDbContext context) : BaseRepository<EmployeeDevice>(context), IEmployeeDeviceRepository
{
    private readonly CompanyPortalDbContext _context = context;

    public async Task<List<EmployeeDevice>> GetBorrowedByEmployeeIdAsync(int employeeId, CancellationToken ct)
    {
        return await _context.EmployeeDevices
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .Include(x => x.Device)
            .Include(x => x.Employee)
            .ToListAsync(ct);
    }

    public IQueryable<EmployeeDevice> GetAllQueryableWithDependencies()
    {
        return _context.EmployeeDevices
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Device);
    }
}