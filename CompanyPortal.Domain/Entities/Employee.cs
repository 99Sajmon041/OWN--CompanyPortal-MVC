using CompanyPortal.Domain.EntityInterface;
using CompanyPortal.Shared.Attributes;

namespace CompanyPortal.Domain.Entities;

[FilterGroup("Zaměstnanec")]
public sealed class Employee : IHasId, IHasActive
{
    [NotFilterable]
    public int Id { get; set; }

    [FilterOrder(1)]
    [FilterLabel("Osobní číslo zaměstnance")]
    public string PersonalNumber { get; set; } = default!;

    [FilterOrder(2)]
    [FilterLabel("Jméno zaměstnance")]
    public string FirstName { get; set; } = default!;

    [FilterOrder(3)]
    [FilterLabel("Příjmení zaměstnance")]
    public string LastName { get; set; } = default!;

    [NotFilterable]
    public string ApplicationUserId { get; set; } = default!;

    public Position Position { get; set; } = default!;

    [NotFilterable]
    public int PositionId { get; set; }

    [FilterOrder(4)]
    [FilterLabel("Stav zaměstnance")]
    public bool IsActive { get; set; }

    [FilterOrder(5)]
    [FilterLabel("Zaměstnanec vytvořen")]
    public DateTime CreatedAt { get; set; }

    [NotFilterable]
    public string ImageUrl { get; set; } = default!;

    [NotFilterable]
    public ICollection<EmployeeDevice> EmployeeDevices { get; set; } = [];
}
