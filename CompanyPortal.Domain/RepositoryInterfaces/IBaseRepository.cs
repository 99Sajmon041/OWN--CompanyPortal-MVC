using CompanyPortal.Domain.EntityInterface;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IBaseRepository<T> where T : class, IHasId, IHasActive
{
    Task AddAsync(T entity, CancellationToken ct);
    IQueryable<T> GetAllQueryable();
    Task<T?> GetWithoutDependenciesAsync(int id, CancellationToken ct);
    Task<List<T>> GetAllAsync(CancellationToken ct);
    Task<List<T>> GetAllActiveAsync(CancellationToken ct);

    //Testing purposes only, not to be used in production code
    bool ExistsRecord<S>(Dictionary<string, string> properties) where S : class;
}
