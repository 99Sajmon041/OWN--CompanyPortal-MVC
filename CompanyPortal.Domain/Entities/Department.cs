namespace CompanyPortal.Domain.Entities;

public sealed class Department
{
    public int Id { get; set; }
    public string DepartmentName { get; set; } = default!;
    public ICollection<Position> Positions { get; set; } = [];
}