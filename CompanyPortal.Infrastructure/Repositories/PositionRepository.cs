using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;
using CompanyPortal.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class PositionRepository(CompanyPortalDbContext context) : BaseRepository<Position>(context), IPositionRepository
{
    private readonly CompanyPortalDbContext _context = context;

    public IQueryable<Position> GetAllQueryableWithDepartment()
    {
        return _context.Positions
            .AsNoTracking()
            .Include(p => p.Department);
    }

    public async Task<Position?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Positions
            .AsNoTracking()
            .Include(p => p.Department)
            .Include(x => x.Employees)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<bool> ExistsAnyWithSameParametersAsync(string positionName, int id, CancellationToken ct)
    {
        return await _context.Positions.AnyAsync(x => x.Id != id && x.PositionName == positionName, ct);
    }

    public async Task<bool> ExistsAnyWithSameParametersAsync(string positionName, CancellationToken ct)
    {
        return await _context.Positions.AnyAsync(x => x.PositionName == positionName, ct);
    }

    public async Task<bool> ExistsManagingPositionInDepartmentAsync(int departmentId, CancellationToken ct)
    {
        return await _context.Positions
            .AnyAsync(x => x.DepartmentId == departmentId &&
                (x.PositionLevel == PositionLevel.GroupLeader
                || x.PositionLevel == PositionLevel.Director
                || x.PositionLevel == PositionLevel.Board),
                ct);
    }
}