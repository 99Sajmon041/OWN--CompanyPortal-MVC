namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IAppTransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken ct);
    Task RollbackAsync(CancellationToken ct);
}
