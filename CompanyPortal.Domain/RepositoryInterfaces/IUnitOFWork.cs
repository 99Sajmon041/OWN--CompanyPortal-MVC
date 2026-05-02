namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IUnitOfWork
{
    IEmployeeRepository EmployeeRepository { get; }
    Task SaveChangesAsync(CancellationToken ct);
}
