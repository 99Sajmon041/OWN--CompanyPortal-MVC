using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class BuildingRepository(CompanyPortalDbContext context) : BaseRepository<Building>(context), IBuildingRepository
{
    private readonly CompanyPortalDbContext _context = context;

    public async Task<Building?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Buildings
            .Include(x => x.Departments)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<bool> ExistsAnyWithSameParametersAsync(string buildingName, string code, int id, CancellationToken ct)
    {
        return await _context.Buildings.AnyAsync(x => x.Id != id && (x.BuildingName == buildingName || x.Code == code), ct);
    }

    public async Task<bool> ExistsAnyWithSameParametersAsync(string buildingName, string code, CancellationToken ct)
    {
        return await _context.Buildings.AnyAsync(x => x.BuildingName == buildingName || x.Code == code, ct);
    }
}
