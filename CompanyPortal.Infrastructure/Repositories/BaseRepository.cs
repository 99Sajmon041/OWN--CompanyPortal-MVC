using CompanyPortal.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Repositories;

public class BaseRepository<T>(CompanyPortalDbContext context) where T : class
{
    private readonly DbSet<T> database = context.Set<T>();

    public async Task AddAsync(T entity)
    {
        await database.AddAsync(entity);
    }

    public IQueryable<T> GetAll()
    {
        return database.AsQueryable();
    }
}
