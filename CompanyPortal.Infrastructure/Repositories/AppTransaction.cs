using CompanyPortal.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class AppTransaction(IDbContextTransaction transaction) : IAppTransaction
{
    public async Task CommitAsync(CancellationToken ct)
    {
        await transaction.CommitAsync(ct);
    }

    public async Task RollbackAsync(CancellationToken ct)
    {
        await transaction.RollbackAsync(ct);
    }

    public async ValueTask DisposeAsync()
    {
        await transaction.DisposeAsync();
    }
}
