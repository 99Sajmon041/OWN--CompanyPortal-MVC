using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class PositionRepository(CompanyPortalDbContext context) : BaseRepository<Position>(context), IPositionRepository
{
    private readonly CompanyPortalDbContext _context = context;
}
