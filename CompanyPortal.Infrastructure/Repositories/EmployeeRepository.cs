using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;
using CompanyPortal.Shared.Enums;
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
    public IQueryable<Employee> GetAllWithPositionAsync()
    {
        return _context.Employees
            .AsNoTracking()
            .Include(x => x.Position);
    }

    public async Task<Employee?> GetByApplicationUserIdAsync(string applicationUserId, CancellationToken ct)
    {
        return await _context.Employees
            .AsNoTracking()
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.ApplicationUserId == applicationUserId, ct);
    }

    public async Task<bool> ExistsActiveEmployeeOnPositionAsync(int positionId, int excludedEmployeeId, CancellationToken ct)
    {
        return await _context.Employees
            .AnyAsync(x =>
                x.PositionId == positionId &&
                x.Id != excludedEmployeeId &&
                x.IsActive &&
                x.Position.PositionLevel != PositionLevel.Member,
                ct);
    }

    public async Task<bool> ExistsActiveEmployeeOnPositionAsync(int positionId, CancellationToken ct)
    {
        return await _context.Employees
            .AnyAsync(x =>
                x.PositionId == positionId &&
                x.IsActive &&
                x.Position.PositionLevel != PositionLevel.Member,
                ct);
    }

    public async Task<string?> GetGroupLeaderNameAsync(int deparmentId, CancellationToken ct)
    {
        return await _context.Employees
            .Where(x =>
                x.IsActive &&
                x.Position.IsActive &&
                x.Position.DepartmentId == deparmentId &&
                x.Position.PositionLevel == PositionLevel.GroupLeader)
            .Select(x => x.FirstName + " " + x.LastName)
            .FirstOrDefaultAsync(ct);
    }
}
