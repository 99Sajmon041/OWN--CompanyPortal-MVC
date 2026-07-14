using CompanyPortal.Domain.EntityInterface;
using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CompanyPortal.Infrastructure.Repositories;

public class BaseRepository<T>(CompanyPortalDbContext context) : IBaseRepository<T> where T : class, IHasId
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
        if (typeof(IHasActive).IsAssignableFrom(typeof(T)) == false)
        {
            throw new Exception($"Typ / Entita: {typeof(T).Name} nepodporuje 'IsActive'.");
        }
            
        return await database
            .Cast<IHasActive>()
            .Where(x => x.IsActive)
            .Cast<T>()
            .ToListAsync(ct);
    }
    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct)
    {
        return await database.AnyAsync(x => x.Id == id, ct);
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