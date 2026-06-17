using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Application.Models.Building;

public sealed class BuildingUpsertViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Název budovy je povinný")]
    [StringLength(50, ErrorMessage = "Název budovy musí mít 5 - 50 znaků",  MinimumLength = 5)]
    public string BuildingName { get; set; } = default!;

    [StringLength(100, ErrorMessage = "Popis budovy může mít maximálně 100 znaků")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Kód budovy je povinný")]
    [StringLength(10, ErrorMessage = "Kód budovy musí mít 5 - 10 znaků", MinimumLength = 5)]
    public string Code { get; set; } = default!;
}