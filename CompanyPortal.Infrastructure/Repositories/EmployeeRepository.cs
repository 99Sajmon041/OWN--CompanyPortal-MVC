using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class EmployeeRepository(CompanyPortalDbContext context) : BaseRepository<Employee>(context), IEmployeeRepository
{
    private readonly CompanyPortalDbContext _context = context;

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Employees
            .Include(x => x.Position)
            .ThenInclude(x => x.Department)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
