using CompanyPortal.Application.Models.Employee;
using CompanyPortal.Shared.Enums;

namespace CompanyPortal.Application.Models.Position;

public sealed class PositionDetailViewModel
{
    public int Id { get; set; }
    public string PositionName { get; set; } = default!;
    public PositionLevel PositionLevel { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = default!;
    public bool IsActive { get; set; }
    public decimal HourlyRate { get; set; }
    public ICollection<EmployeeViewModel> Employees { get; set; } = [];
}
