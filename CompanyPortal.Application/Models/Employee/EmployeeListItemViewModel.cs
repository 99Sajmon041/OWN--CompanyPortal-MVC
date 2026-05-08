namespace CompanyPortal.Application.Models.Employee;

public sealed class EmployeeListItemViewModel
{
    public int Id { get; set; }
    public string PersonalNumber { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string ApplicationUserId { get; set; } = default!;
    public string PositionName { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
