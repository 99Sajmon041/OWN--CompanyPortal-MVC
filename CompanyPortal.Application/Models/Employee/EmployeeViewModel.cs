using CompanyPortal.Application.Models.EmployeeDevice;

namespace CompanyPortal.Application.Models.Employee;

public sealed class EmployeeViewModel
{
    public int Id { get; set; }
    public string PersonalNumber { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public string ApplicationUserId { get; set; } = default!;
    public string PositionName { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? SupervisorName { get; set; }
    public string ImageUrl { get; set; } = default!;
    public ICollection<string> Roles { get; set; } = [];
    public ICollection<EmployeeDeviceViewModel> EmployeeDevices { get; set; } = [];
}
