using CompanyPortal.Shared.Attributes;
using Microsoft.AspNetCore.Identity;

namespace CompanyPortal.Infrastructure.AppUser;

[FilterGroup("Uživatel")]
public sealed class ApplicationUser : IdentityUser
{
    [FilterOrder(1)]
    [FilterLabel("Název domény")]
    public string DomainUserName { get; set; } = default!;

    [FilterOrder(2)]
    [FilterLabel("Stav uživatele")]
    public bool IsActive { get; set; }

    [FilterOrder(3)]
    [FilterLabel("Uživatel vyvořen")]
    public DateTime CreatedAt { get; set; }

    [FilterOrder(4)]
    [FilterLabel("Zaměstnanec. profil dokončen")]
    public bool IsEmployeeProfileCompleted { get; set; }

    [FilterOrder(5)]
    [FilterLabel("Platnost pracovního poměru")]
    public DateTime? AccessValidUntil { get; set; }
}