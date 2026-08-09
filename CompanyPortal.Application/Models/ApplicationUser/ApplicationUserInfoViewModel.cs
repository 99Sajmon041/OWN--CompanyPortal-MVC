using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.ApplicationUser;

public sealed class ApplicationUserViewModel
{
    [Display(Name = "ID")]
    public string Id { get; set; } = default!;

    [Display(Name = "Už. jméno")]
    public string UserName { get; set; } = default!;

    [Display(Name = "E-mail")]
    public string Email { get; set; } = default!;

    [Display(Name = "Doménové už. jméno")]
    public string DomainUserName { get; set; } = default!;

    [Display(Name = "Profil dokončen")]
    public bool IsEmployeeProfileCompleted { get; set; }

    [Display(Name = "Aktivní")]
    public bool IsActive { get; set; }

    [Display(Name = "Vytvořen")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Přístup platný do")]
    public DateTime? AccessValidUntil { get; set; }
}
