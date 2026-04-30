using Microsoft.AspNetCore.Identity;

namespace CompanyPortal.Infrastructure.AppUser;

public sealed class ApplicationUser : IdentityUser
{
    public string DomainUserName { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsEmployeeProfileCompleted { get; set; }
}