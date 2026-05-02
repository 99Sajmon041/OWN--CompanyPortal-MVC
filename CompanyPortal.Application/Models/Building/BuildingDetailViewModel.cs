using CompanyPortal.Application.Models.Department;

namespace CompanyPortal.Application.Models.Building;

public sealed class BuildingDetailViewModel
{
    public int Id { get; set; }
    public string BuildingName { get; set; } = default!;
    public string? Description { get; set; }
    public string Code { get; set; } = default!;
    public bool IsActive { get; set; }
    public ICollection<DepartmentListItemViewModel> Departments { get; set; } = [];
}
