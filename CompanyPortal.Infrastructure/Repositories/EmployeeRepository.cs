using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class EmployeeRepository(CompanyPortalDbContext context) : BaseRepository<Employee>(context), IEmployeeRepository
{
    // Add hotovo
    // List - přes dynamický pager (reší service, jen vrátim IQueryable z baseRepa)
    // Update opět řeší service
    // Change Status (deaktivate / activate) - opět řeší service

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Employees
            .Include(x => x.Position)
            .ThenInclude(x => x.Department)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
