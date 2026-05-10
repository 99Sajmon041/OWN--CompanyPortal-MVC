namespace CompanyPortal.Domain.RepositoryInterfaces;

public interface IUnitOfWork
{
    IEmployeeRepository EmployeeRepository { get; }
    IBuildingRepository BuildingRepository { get; }
    IPositionRepository PositionRepository { get; }
    IDepartmentRepository DepartmentRepository { get; }
    Task SaveChangesAsync(CancellationToken ct);
    Task<IAppTransaction> BeginTransactionAsync(CancellationToken ct);
}
