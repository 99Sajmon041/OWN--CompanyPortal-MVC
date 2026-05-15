using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyPortal.Application.Models.Employee;

public sealed class EmployeeProfileViewModel
{
    public int Id { get; set; }
    public string PersonalNumber { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string ApplicationUserId { get; set; } = default!;
    public string PositionName { get; set; } = default!;
    public string DepartmentName { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? SupervisorName { get; set; }
    public string ImageUrl { get; set; } = default!;
    public ICollection<SelectListItem> SubordinateEmployees { get; set; } = [];
    public ICollection<string> Roles { get; set; } = [];
}
