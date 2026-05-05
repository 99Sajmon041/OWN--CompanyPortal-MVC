using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class DepartmentRepository(CompanyPortalDbContext context) : BaseRepository<Department>(context), IDepartmentRepository
{
    private readonly CompanyPortalDbContext _context = context;
    public IQueryable<Department> GetAllQueryableWithBuilding()
    {
        return _context.Departments
            .AsNoTracking()
            .Include(x => x.Building);
    }

    public async Task<Department?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Departments
            .Include(x => x.Building)
            .Include(x => x.Positions)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
