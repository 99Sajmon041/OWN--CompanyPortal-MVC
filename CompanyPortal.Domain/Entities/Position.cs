namespace CompanyPortal.Domain.Entities;

public sealed class Position
{
    public int Id { get; set; }
    public string PositionName { get; set; } = default!;
    public Department Department { get; set; } = default!;
    public int DepartmentId { get; set; }
    public ICollection<Employee> Employees { get; set; } = [];
}
