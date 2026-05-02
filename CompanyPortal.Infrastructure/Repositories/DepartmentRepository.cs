using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class DepartmentRepository(CompanyPortalDbContext context) : BaseRepository<Department>(context), IDepartmentRepository
{
    private readonly CompanyPortalDbContext _context = context;

}
