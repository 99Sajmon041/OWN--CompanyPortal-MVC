using CompanyPortal.Domain.EntityInterface;

namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IBaseRepository<T> where T : class, HasId
{
    Task AddAsync(T entity, CancellationToken ct);
    IQueryable<T> GetAll();
    Task<T?> GetWithoutDependenciesAsync(int id, CancellationToken ct);
}
