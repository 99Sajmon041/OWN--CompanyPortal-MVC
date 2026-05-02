using CompanyPortal.Domain.RepositoryInterfaces;
using CompanyPortal.Infrastructure.Database;

namespace CompanyPortal.Infrastructure.Repositories;

public sealed class UnitOfWork(CompanyPortalDbContext context) : IUnitOfWork
{
    private IEmployeeRepository? employeeRepository;
    private IBuildingRepository? buildingRepository;
    private IPositionRepository? positionRepository;
    private IDepartmentRepository? departmentRepository;

    public IEmployeeRepository EmployeeRepository => employeeRepository ??= new EmployeeRepository(context);
    public IBuildingRepository BuildingRepository => buildingRepository ??= new BuildingRepository(context);
    public IPositionRepository PositionRepository => positionRepository ??= new PositionRepository(context);
    public IDepartmentRepository DepartmentRepository => departmentRepository ??= new DepartmentRepository(context);


    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await context.SaveChangesAsync(ct);
    }
}
