using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class UnitOfWork(CompanyPortalDbContext context) : IUnitOfWork
{
    private IEmployeeRepository? employeeRepository;

    public IEmployeeRepository EmployeeRepository => employeeRepository ??= new EmployeeRepository(context);

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await context.SaveChangesAsync(ct);
    }
}
