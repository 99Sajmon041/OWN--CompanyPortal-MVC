namespace CompanyPortal.Application.Models.ApplicationUser;

public sealed class ApplicationUserViewModel
{
    public string Id { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string DomainUserName { get; set; } = default!;
    public bool IsEmployeeProfileCompleted { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? AccessValidUntil { get; set; }
}
