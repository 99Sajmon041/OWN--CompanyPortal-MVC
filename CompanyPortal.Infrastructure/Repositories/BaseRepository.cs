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

    public IQueryable<T> GetAllQueryable()
    {
        return database.AsNoTracking();
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


    //Testing purposes only, not to be used in production code
    public bool ExistsRecord<S>(Dictionary<string, string> properties) where S : class
    {
        var records = context.Set<S>().AsNoTracking().ToList();
        var recordProperties = typeof(S).GetProperties();

        foreach (var record in records)
        {
            foreach (var property in properties)
            {
                var propertyInfo = recordProperties.FirstOrDefault(x => x.Name == property.Key);

                if (propertyInfo is null)
                {
                    continue;
                }

                string? recordValue = propertyInfo.GetValue(record)?.ToString();

                if (recordValue == property.Value)
                {
                    return true;
                }
            }
        }

        return false;
    }
}