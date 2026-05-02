using CompanyPortal.Domain.EntityInterface;

namespace CompanyPortal.Domain.Entities;

public sealed class Building : HasId
{
    public int Id { get; set; }
    public string BuildingName { get; set; } = default!;
    public string? Description { get; set; }
    public string Code { get; set; } = default!;
    public bool IsActive { get; set; }
    public ICollection<Department> Departments { get; set; } = [];
}
