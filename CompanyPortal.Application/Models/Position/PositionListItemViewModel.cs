using CompanyPortal.Shared.Enums;

namespace CompanyPortal.Application.Models.Position;

public sealed class PositionListItemViewModel
{
    public int Id { get; set; }
    public string PositionName { get; set; } = default!;
    public PositionLevel PositionLevel { get; set; }
    public bool IsActive { get; set; }
    public decimal HourlyRate { get; set; }
    public string DepartmentName { get; set; } = default!;
}
