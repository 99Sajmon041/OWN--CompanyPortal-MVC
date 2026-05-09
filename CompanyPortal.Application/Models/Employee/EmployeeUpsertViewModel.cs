using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompanyPortal.Application.Models.Employee;

public sealed class EmployeeUpsertViewModel
{
    public int Id { get; set; }
    public string PersonalNumber { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string ApplicationUserId { get; set; } = default!;
    public List<SelectListItem> ApplicationUsers { get; set; } = [];
    public int PositionId { get; set; }
    public List<SelectListItem> Positions { get; set; } = [];
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}