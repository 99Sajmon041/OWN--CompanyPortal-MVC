using CompanyPortal.Domain.EntityInterface;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IBaseRepository<T> where T : class, IHasId, IHasActive
{
    Task AddAsync(T entity, CancellationToken ct);
    IQueryable<T> GetAll();
    Task<T?> GetWithoutDependenciesAsync(int id, CancellationToken ct);
    Task<List<T>> GetAllAsync(CancellationToken ct);
    Task<List<T>> GetAllActiveAsync(CancellationToken ct);

}
