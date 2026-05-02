using CompanyPortal.Domain.EntityInterface;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Repositories;

public class BaseRepository<T>(CompanyPortalDbContext context) : IBaseRepository<T> where T : class, IHasId, IHasActive
{
    private readonly DbSet<T> database = context.Set<T>();

    public async Task AddAsync(T entity, CancellationToken ct)
    {
        await database.AddAsync(entity, ct);
    }

    public IQueryable<T> GetAll()
    {
        return database.AsQueryable();
    }

    public async Task<T?> GetWithoutDependenciesAsync(int id, CancellationToken ct)
    {
        return await database.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken ct)
    {
        return await database.ToListAsync(ct);
    }

    public async Task<List<T>> GetAllActiveAsync(CancellationToken ct)
    {
        return await database
            .Where(x => x.IsActive)
            .ToListAsync(ct);
    }
}