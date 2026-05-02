using CompanyPortal.Domain.EntityInterface;

namespace CompanyPortal.Domain.Entities;

public sealed class Employee : HasId
{
    public int Id { get; set; }
    public string PersonalNumber { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string ApplicationUserId { get; set; } = default!;
    public Position Position { get; set; } = default!;
    public int PositionId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
