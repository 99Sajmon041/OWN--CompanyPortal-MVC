namespace CompanyPortal.Application.Models.Employee;

public sealed class EmployeeShortViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public string PositionName { get; set; } = default!;
}