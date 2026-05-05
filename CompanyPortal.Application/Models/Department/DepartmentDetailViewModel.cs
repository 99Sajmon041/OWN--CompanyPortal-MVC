using CompanyPortal.Application.Models.Position;

namespace CompanyPortal.Application.Models.Department;

public sealed class DepartmentDetailViewModel
{
    public int Id { get; set; }
    public string DepartmentName { get; set; } = default!;
    public bool IsActive { get; set; }
    public string BuildingName { get; set; } = default!;
    public ICollection<PositionListItemViewModel> Positions { get; set; } = [];
}