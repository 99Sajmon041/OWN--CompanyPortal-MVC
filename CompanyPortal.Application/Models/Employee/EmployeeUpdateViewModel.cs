using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Employee;

public sealed class EmployeeUpdateViewModel
{
    public int Id { get; set; }
    public string PersonalNumber { get; set; } = default!;

    [Required(ErrorMessage = "Křestní jméno je povinné.")]
    [StringLength(50, ErrorMessage = "Křestní jméno musí mít 2 - 50 znaků", MinimumLength = 2)]
    public string FirstName { get; set; } = default!;

    [StringLength(50, ErrorMessage = "Příjmení musí mít 2 - 50 znaků", MinimumLength = 2)]
    [Required(ErrorMessage = "Příjmení je povinné.")]
    public string LastName { get; set; } = default!;
    public IFormFile? ImageFile { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
